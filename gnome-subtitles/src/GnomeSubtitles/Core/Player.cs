/*
 * This file is part of Gnome Subtitles.
 * Copyright (C) 2006-2007 Pedro Castro
 *
 * Gnome Subtitles is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * Gnome Subtitles is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
 */

using Gtk;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace GnomeSubtitles {

public delegate float PlayerGetPositionFunc (); //Represents a function that gets the current player position
public delegate void PlayerPositionChangedFunc (float position); //Represents a function that handles changes in the position
public delegate void PlayerEndReachedFunc (); //Represents a function that handles reaching of the end in the player

public class Player {
	private Socket socket = null;
	private Process process = null;
	private PlayerPositionWatcher position = null;
	
	private string filename = String.Empty;
	private float frameRate = 0;
	
	/* Events */
	public event EventHandler EndReached = null;

	public Player () {
		CreateSocket();
		position = new PlayerPositionWatcher(GetPosition, EmitEndReachedEvent);
	}
	
	public Socket Widget {
		get { return socket; }
	}
	
	/* Public properties */
	
	public bool Paused {
		get { return position.Paused; }
	}
	
	/// <summary>The aspect ratio.</summary>	
	public float AspectRatio {
		get {
			float width = GetAsInt32("pausing_keep get_property width");
			float height = GetAsInt32("pausing_keep get_property height");
			return width / height;
		}
	}
	
	/// <summary>The length of the video, in seconds.</summary>
	public float Length {
		get { return position.Length; }
	}
	
	public float FrameRate {
		get { return frameRate; }
	}
	
	public PlayerPositionChangedFunc OnPositionChanged {
		set { position.OnPlayerPositionChanged = value; }
	}


	/* Public methods */

	/// <summary>Opens a video file.</summary>
	/// <exception cref="PlayerNotFoundException">Thrown if the player executable was not found.</exception>
	/// <exception cref="PlayerCouldNotOpenVideoException">Thrown if the player could not open the video.</exception>
	public void Open (string filename) {
		this.filename = filename;
		
		position.Stop();
	
		StartNewProcess(filename);
		bool couldStart = ClearOutput();
		if (!couldStart)
			throw new PlayerCouldNotOpenVideoException();
		
		position.Enable(GetLength());
		
		frameRate = GetFrameRate();
	}

	/// <summary>Closes the video.</summary>
	public void Close () {
		position.Disable();
		TerminateProcess();
		
		this.filename = String.Empty;
		this.frameRate = 0;
	}
	
	public void SeekStart () {
		Exec("pausing seek 0 2");
	}
	
	public void Play () {
		if (position.EndReached) {
			EmitEndReachedEvent();
			return;
		}
		
		if (position.Paused) {
			Exec("pause");
			position.Start();
		}
	}
	
	public void Pause () {
		if (position.EndReached) {
			EmitEndReachedEvent();
			return;
		}
		
		if (!position.Paused) {
			position.Stop();
			Exec("pause");
		}
	}
	
	public void Seek (float newPosition) {
		Exec("pausing_keep seek " + newPosition + " 2");
		position.Check();
	}
	
	public void Rewind (float decrement) {
		if (position.EndReached) { //Seek to near the end
			float length = position.Length;
			float nearEndPosition = (length > 5) ? length - 5 : 0;
			Seek(nearEndPosition);
		}
		else {
			Exec("pausing_keep seek -" + decrement + " 0");
			position.Check();
		}
	}
	
	public void Forward (float increment) {
		if (position.EndReached) {
			EmitEndReachedEvent();
			return;
		}
	
		Exec("pausing_keep seek " + increment + " 0");
		position.Check();
	}
	

	/* Private methods */
	
	private void RestartPlayer () {
		StartNewProcess(filename);
		ClearOutput();
		SeekStart();
	}
	
	private void CreateSocket () {
		socket = new Socket();
		socket.ModifyBg(StateType.Normal, socket.Style.Black);	
	}
	
	/// <summary>Starts a new MPlayer process on slave mode and idle.</summary>
	/// <exception cref="PlayerNotFoundException">Thrown if the player executable was not found.</exception>
	private void StartNewProcess (string filename) {
		/* Configure startup of new process */
		Process newProcess = new Process();
		newProcess.StartInfo.FileName = "mplayer";

		//newProcess.StartInfo.Arguments = "-wid " + socket.Id + " -osdlevel 3 -fontconfig -subfont-autoscale 2 -quiet -nomouseinput -slave " + filename;
		newProcess.StartInfo.Arguments = "-wid " + socket.Id + " -osdlevel 0 -noautosub -quiet -nomouseinput -slave " + filename;
		if (!newProcess.StartInfo.EnvironmentVariables.ContainsKey("TERM")) {
			newProcess.StartInfo.EnvironmentVariables.Add("TERM", "xterm");
		}
		newProcess.StartInfo.UseShellExecute = false;
		newProcess.StartInfo.RedirectStandardInput = true;
		newProcess.StartInfo.RedirectStandardOutput = true;
		newProcess.EnableRaisingEvents = true;
		newProcess.Exited += OnProcessExited;

		try {
			newProcess.Start();
		} catch (Win32Exception) {
			throw new PlayerNotFoundException();
		}
		process = newProcess;
	}
	
	/// <summary>Terminates the current running process, if it exists.</summary>
	/// <remarks>Waits for the process to end, and kills it if it doesn't.</remarks>
	private void TerminateProcess () {
		if (process != null) {
			process.Exited -= OnProcessExited;
			
			try {
				Exec("quit");
			}
			catch (IOException) {
				//Do nothing
			}
			
			bool exited = process.WaitForExit(1000); //Wait 1 second for exit
			if (!exited) {
				try {
					process.Kill();
				}
				catch (Exception) {
					//Do nothing
				}
			}

			process = null;
		}
	}
	
	/// <summary>Gets the current position, in seconds.</summary>
	/// <returns>The current position, in seconds, or -1 if the end has been reached.</returns>
	private float GetPosition () {
		try {
			if (position.Paused)
				return GetAsFloat("pausing get_time_pos");
			else
				return GetAsFloat("get_time_pos");
		}
		catch (FormatException) { //Reached the end
			return -1;
		}
	}
	
	private float GetLength () {
		if (position.Paused)
			return GetAsFloat("pausing get_time_length");
		else
			return GetAsFloat("get_time_length");
	}
	
	private float GetFrameRate () {
		if (position.Paused)
			return GetAsFloat("pausing get_property fps");
		else
			return GetAsFloat("get_property fps");
	}
	
	private void Exec (string command) {
		process.StandardInput.WriteLine(command);
	}
	
	private string Get (string command) {
		Exec(command);

		string line = process.StandardOutput.ReadLine();
		int index = line.LastIndexOf("=");
		return (index == -1 ? String.Empty : line.Substring(index + 1));
	}
	
	private int GetAsInt32 (string command) {
		string text = Get(command);
		return Convert.ToInt32(text);
	}
	
	/// <summary>Gets a float as the response to a command.</summary>
	/// <param name="command">The command to execute.</param>
	/// <returns>The response for the command parsed as a float.</returns>
	/// <exception cref="FormatException">Thrown when the response cannot be parsed as a float.</exception> 
	private float GetAsFloat (string command) {
		string text = Get(command);
		NumberFormatInfo invariant = NumberFormatInfo.InvariantInfo;
		return (float)Convert.ToDouble(text, invariant);
	}

	/// <summary>Clears the current output.</summary>
	/// <returns>Whether output could be cleared or not. In case not, it means the player could not be started correcty or may have terminated.</returns>
	/// <remarks>This uses a hack to detect the end of the output, as the StreamReader could not detect it correctly.
	/// It executes a command and uses the result of that command to detect the end of output.</remarks>
	private bool ClearOutput () {
		Exec("get_vo_fullscreen");
		StreamReader reader = process.StandardOutput;
		while (true) {
			string line = reader.ReadLine();
			if (line == null) //Got end of stream
				return false;

			if (line.StartsWith("ANS_VO_FULLSCREEN"))
				break;
		}
		return true;
	}
	
	/* Event members */
	
	private void EmitEndReachedEvent () {
		if (EndReached != null)
			EndReached(this, EventArgs.Empty);
	}
	
	private void OnProcessExited (object o, EventArgs args) {
		position.Stop();
		
		/* Check if it is near the end, meaning playback has ended */
		if (position.NearEndReached) {
			position.SetEndReached();
			RestartPlayer();
		}
	}
	
}

}
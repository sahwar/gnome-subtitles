/*
 * This file is part of Gnome Subtitles.
 * Copyright (C) 2006 Pedro Castro
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

using System;
using Glade;
using Gtk;
using SubLib;

namespace GnomeSubtitles {


public class TimingsShiftDialog : GladeDialog {
	private TimingMode timingMode;

	/* Constant strings */
	private const string gladeFilename = "TimingsShiftDialog.glade";

	/* Widgets */
	[WidgetAttribute]
	private Label timingModeLabel;
	[WidgetAttribute]
	private SpinButton spinButton;
	[WidgetAttribute]
	private RadioButton allSubtitlesRadioButton;

	public TimingsShiftDialog () : base(gladeFilename){
		timingMode = Global.TimingMode;
		SetSpinButton();
		UpdateForTimingMode(timingMode);
	}
	
	private void SetSpinButton () {
		spinButton.WidthRequest = Util.SpinButtonTimeWidth(spinButton);
		spinButton.Alignment = 0.5f;
	}

	private void UpdateForTimingMode (TimingMode timingMode) {
		Util.SetSpinButtonTimingMode(spinButton, timingMode, true);
		if (timingMode == TimingMode.Times) {
			timingModeLabel.Markup = "<b>Time</b>";
			spinButton.Value = 0;
		}
	}

	#pragma warning disable 169		//Disables warning about handlers not being used
	
	private void OnResponse (object o, ResponseArgs args) {
		if (args.ResponseId == ResponseType.Ok) {
			SelectionType selectionType = (allSubtitlesRadioButton.Active ? SelectionType.All : SelectionType.Simple);
			
			if (timingMode == TimingMode.Times) {
				TimeSpan time = TimeSpan.Parse(spinButton.Text);
				Global.CommandManager.Execute(new ShiftTimingsCommand(time, selectionType));
			}
			else {
				int frames = (int)spinButton.Value;
				Global.CommandManager.Execute(new ShiftTimingsCommand(frames, selectionType));
			}
		}
		CloseDialog();
	}

}

}
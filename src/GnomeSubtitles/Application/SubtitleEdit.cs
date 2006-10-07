/*
 * This file is part of Gnome Subtitles, a subtitle editor for Gnome.
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

using Gtk;
using System;
using System.Collections;
using SubLib;

namespace GnomeSubtitles {

public class SubtitleEdit : GladeWidget {
	private SpinButton startSpinButton = null;
	private SpinButton endSpinButton = null;
	private SpinButton durationSpinButton = null;
	private TextView textView = null;
	private	Widget hBox = null;
	
	private TextTag scaleTag = new TextTag("scale");
	private TextTag boldTag = new TextTag("bold");
	private TextTag italicTag = new TextTag("italic");
	private TextTag underlineTag = new TextTag("underline");
	private ArrayList subtitleTags = new ArrayList(4); //4 not to resize with 3 items
	
	private Subtitle subtitle = null;

	public SubtitleEdit(GUI gui, Glade.XML glade) : base(gui, glade){
		startSpinButton = (SpinButton)GetWidget(WidgetNames.StartSpinButton);
		endSpinButton = (SpinButton)GetWidget(WidgetNames.EndSpinButton);
		durationSpinButton = (SpinButton)GetWidget(WidgetNames.DurationSpinButton);
		textView = (TextView)GetWidget(WidgetNames.SubtitleTextView);
		hBox = GetWidget(WidgetNames.SubtitleEditHBox);
		
    	startSpinButton.WidthRequest = Utility.SpinButtonTimeWidth(startSpinButton);
    	startSpinButton.Alignment = 0.5f;
    	endSpinButton.Alignment = 0.5f;
    	durationSpinButton.Alignment = 0.5f;   
    		 	
    	scaleTag.Scale = Pango.Scale.XLarge;
    	boldTag.Weight = Pango.Weight.Bold;
    	italicTag.Style = Pango.Style.Italic;
    	underlineTag.Underline = Pango.Underline.Single;
    	textView.Buffer.TagTable.Add(scaleTag);
    	textView.Buffer.TagTable.Add(boldTag);
    	textView.Buffer.TagTable.Add(italicTag);
    	textView.Buffer.TagTable.Add(underlineTag);
    	
    	SetTimesMode(); //Initial timing mode is Times
    }
    
    public void GetEditableWidgets (out SpinButton startSpinButton, out SpinButton endSpinButton,
    		out SpinButton durationSpinButton, out TextView textView) {
    	
    	startSpinButton = this.startSpinButton;
    	endSpinButton = this.endSpinButton;
    	durationSpinButton = this.durationSpinButton;
    	textView = this.textView;   	
    }
    
    public void NewDocument (bool wasLoaded) {
    	if (!wasLoaded)
    		hBox.Sensitive = true;
    }
    
    public void BlankStartUp () {
    	ClearFields();
    }
    
    public bool Sensitive {
    	set {
			if (hBox.Sensitive == value)
				return;

			if (value == false)
				ClearFields();

			hBox.Sensitive = value;
    	}
    }
    
    public void ToggleTimingMode (TimingMode mode) { 	
    	SetTimingMode(mode);
	   	LoadTimings(mode);
    }

    public void LoadSubtitle (Subtitle subtitle) {
	   	this.Sensitive = true;
    	this.subtitle = subtitle;
		LoadTimings(GUI.Core.TimingMode);
		LoadTags();
    	LoadText();
    }
    
    public void ReloadSubtitle () {
    	LoadSubtitle(subtitle);
    }

   	public void LoadStartTiming () {
   		LoadStartTiming(GUI.Core.TimingMode);
   	}
    
	public void LoadEndTiming () {
   		LoadEndTiming(GUI.Core.TimingMode);
   	}
    
    public void LoadDurationTiming () {
   		LoadDurationTiming(GUI.Core.TimingMode);
   	}
    
    public void LoadText () {
    	textView.Buffer.Changed -= OnBufferChanged; 
    	textView.Buffer.Text = subtitle.Text.Get();    		
		ApplyLoadedTags();   
		textView.Buffer.Changed += OnBufferChanged; 
    }
    
    public void LoadTags () {
    	subtitleTags.Clear();
    	if (subtitle.Style.Bold)
    		subtitleTags.Add(boldTag);
    	if (subtitle.Style.Italic)
    		subtitleTags.Add(italicTag);
    	if (subtitle.Style.Underline)
    		subtitleTags.Add(underlineTag);
    }
    
    public void ApplyLoadedTags () {
    	TextBuffer buffer = textView.Buffer;
    	TextIter start = buffer.StartIter;
    	TextIter end = buffer.EndIter;
    	buffer.ApplyTag(scaleTag, start, end);
    	foreach (TextTag tag in subtitleTags)
		SetTag(tag, start, end, true);
    }
    
    public void TextGrabFocus () {
    	textView.GrabFocus();
    }
    
    public void StartSpinButtonGrabFocus () {
    	startSpinButton.GrabFocus();
    }
    
    public void EndSpinButtonGrabFocus () {
    	endSpinButton.GrabFocus();
    }
    
    public void DurationSpinButtonGrabFocus () {
    	durationSpinButton.GrabFocus();
    }
    
    /* Private Methods */
    
    private void ClearFields () {
   		DisconnectTextBufferChangedSignal();
    	textView.Buffer.Text = String.Empty;
    	ConnectTextBufferChangedSignal();
    	
    	DisconnectSpinButtonsChangedSignals();
		startSpinButton.Text = String.Empty;
		endSpinButton.Text = String.Empty;
		durationSpinButton.Text = String.Empty;
    	ConnectSpinButtonsChangedSignals();
    }
    
	private void SetTag (TextTag tag, TextIter start, TextIter end, bool activate) {
    		if (activate)
	    		textView.Buffer.ApplyTag(tag, start, end);
	    	else
	    		textView.Buffer.RemoveTag(tag, start, end);
    }
    
    private void ConnectTextBufferChangedSignal () {
		textView.Buffer.Changed += OnBufferChanged; 
    }
    
    private void DisconnectTextBufferChangedSignal () {
		textView.Buffer.Changed -= OnBufferChanged; 
    }
    
    private void ConnectSpinButtonsChangedSignals () {
    	startSpinButton.ValueChanged += OnStartValueChanged;
    	endSpinButton.ValueChanged += OnEndValueChanged;
    	durationSpinButton.ValueChanged += OnDurationValueChanged;
    }
    
    private void DisconnectSpinButtonsChangedSignals () {
    	startSpinButton.ValueChanged -= OnStartValueChanged;
   		endSpinButton.ValueChanged -= OnEndValueChanged;
   		durationSpinButton.ValueChanged -= OnDurationValueChanged;
   	}
   	
   	private void SetTimingMode(TimingMode mode) {
   		if (mode == TimingMode.Frames)
   			SetFramesMode();
   		else
   			SetTimesMode();
   	}
    
	private void SetFramesMode () {
		SetFramesMode(startSpinButton);
	    SetFramesMode(endSpinButton);
	    SetFramesMode(durationSpinButton);
	}
	
	private void SetTimesMode () {
	    SetTimesMode(startSpinButton);
	    SetTimesMode(endSpinButton);
	    SetTimesMode(durationSpinButton);
	}
    
    private void SetTimesMode (SpinButton spinButton) {
    	spinButton.Input += OnTimeInput;
		spinButton.Output += OnTimeOutput;
		spinButton.Adjustment.StepIncrement = 100;
		spinButton.Adjustment.Upper = 86399999;
	}
	
	private void SetFramesMode (SpinButton spinButton) {
		spinButton.Input -= OnTimeInput;
    	spinButton.Output -= OnTimeOutput;
    	spinButton.Adjustment.StepIncrement = 1;
    	spinButton.Adjustment.Upper = 3000000;
	}
	
	private void LoadTimings (TimingMode mode) {
    	if (subtitle == null)
    		return;
    			
		LoadStartTiming(mode);
		LoadEndTiming(mode);
		LoadDurationTiming(mode);
    }
	
	private void LoadStartTiming (TimingMode mode) {
    	startSpinButton.ValueChanged -= OnStartValueChanged;
    		
    	if (mode == TimingMode.Frames)
    		startSpinButton.Value = subtitle.Frames.Start;
    	else
    		startSpinButton.Value = subtitle.Times.Start.TotalMilliseconds;

   		startSpinButton.ValueChanged += OnStartValueChanged;
   	}
   	
	private void LoadEndTiming (TimingMode mode) {
   		endSpinButton.ValueChanged -= OnEndValueChanged;
    		
    	if (mode == TimingMode.Frames)
    		endSpinButton.Value = subtitle.Frames.End;
    	else
    		endSpinButton.Value = subtitle.Times.End.TotalMilliseconds;

   		endSpinButton.ValueChanged += OnEndValueChanged;
	}
	
	private void LoadDurationTiming (TimingMode mode) {
    	durationSpinButton.ValueChanged -= OnDurationValueChanged;

    	if (mode == TimingMode.Frames)
    		durationSpinButton.Value = subtitle.Frames.Duration;
    	else
    		durationSpinButton.Value = subtitle.Times.Duration.TotalMilliseconds;

    	durationSpinButton.ValueChanged += OnDurationValueChanged;
    }
	
	/* Event Handlers */
    
	private void OnTimeInput (object o, InputArgs args) {
		SpinButton spinButton = o as SpinButton;
		try {
			args.NewValue = Utility.TimeTextToMilliseconds(spinButton.Text);
		}
		catch (Exception) {
			args.NewValue = spinButton.Value;
		}
		args.RetVal = 1;
	}
	
	private void OnTimeOutput (object o, OutputArgs args) {
		SpinButton spinButton = o as SpinButton;
		spinButton.Numeric = false;
		spinButton.Text = Utility.MillisecondsToTimeText((int)spinButton.Value);
		spinButton.Numeric = true;
		args.RetVal = 1;
	}
	
	private void OnBufferChanged (object o, EventArgs args) {
		ApplyLoadedTags();
		GUI.Core.CommandManager.Execute(new ChangeTextCommand(GUI, (o as TextBuffer).Text));
	}

	private void OnStartValueChanged (object o, EventArgs args) {
		if (GUI.Core.TimingModeIsFrames)
			GUI.Core.CommandManager.Execute(new ChangeStartCommand(GUI, (int)startSpinButton.Value));
		else
			GUI.Core.CommandManager.Execute(new ChangeStartCommand(GUI, TimeSpan.FromMilliseconds(startSpinButton.Value)));
	}
	
	private void OnEndValueChanged (object o, EventArgs args) {
		if (GUI.Core.TimingModeIsFrames)
			GUI.Core.CommandManager.Execute(new ChangeEndCommand(GUI, (int)endSpinButton.Value));
		else
			GUI.Core.CommandManager.Execute(new ChangeEndCommand(GUI, TimeSpan.FromMilliseconds(endSpinButton.Value)));
	}
	
	private void OnDurationValueChanged (object o, EventArgs args) {
		if (GUI.Core.TimingModeIsFrames)
			GUI.Core.CommandManager.Execute(new ChangeDurationCommand(GUI, (int)durationSpinButton.Value));
		else
			GUI.Core.CommandManager.Execute(new ChangeDurationCommand(GUI, TimeSpan.FromMilliseconds(durationSpinButton.Value)));
	}

}

}

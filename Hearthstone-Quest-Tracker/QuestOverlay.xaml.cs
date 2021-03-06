﻿/*
 * Created by SharpDevelop.
 * User: Burhanuddin
 * Date: 02-12-2017
 * Time: 02:07 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.Utility.Logging;

namespace Hearthstone_Quest_Tracker
{
	/// <summary>
	/// Stuff overlays do
	/// </summary>
	public partial class QuestOverlay : UserControl
	{
		// Initialization: just clear all fields
		public QuestOverlay()
		{
			InitializeComponent();
			
			classBlock1.Text = "";
			classBlock2.Text = "";
			classBlock3.Text = "";
			playedBlock1.Text = "";
			playedBlock2.Text = "";
			playedBlock3.Text = "";
			
			Log.Info("----- Display did get initialized -----");
		}
		
		// Displays/Hides the overlay in a match and sets its location on screen
		// TODO: if count is < 3 shorten the overlay
		public void UpdatePosition(int qcount)
		{
			this.Visibility = (qcount > 0) ? Visibility.Visible : Visibility.Hidden;
			Canvas.SetTop(this, (Core.OverlayWindow.Height * 85 / 100)-this.Height);
			Canvas.SetRight(this, 0);
			Log.Info("----- Display tried to get shown -----");
		}

		// Erases everything and puts the quest_list back in overlay
		// Also calls updatePosition
		// TODO: Add a blink effect
		public void UpdateQuests(List<Quest> quest_list)
		{
			classBlock1.Text = "";
			classBlock2.Text = "";
			classBlock3.Text = "";
			playedBlock1.Text = "";
			playedBlock2.Text = "";
			playedBlock3.Text = "";
			
			switch(quest_list.Count)
			{
				case 3:
					classBlock3.Text = LabelCorrector(quest_list[2].quest_name);
					playedBlock3.Text = quest_list[2].count.ToString();
					goto case 2;
				
				case 2:
					classBlock2.Text = LabelCorrector(quest_list[1].quest_name);
					playedBlock2.Text = quest_list[1].count.ToString();
					goto case 1;
				
				case 1:
					classBlock1.Text = LabelCorrector(quest_list[0].quest_name);
					playedBlock1.Text = quest_list[0].count.ToString();
					break;
				
				case 0:
					break;
				
				default:
					Log.Info("----- Invalid quests number -----");
					break;
			}		
			UpdatePosition(quest_list.Count);
		}
		
		// Modifies the quest names to better fit the overlay
		private string LabelCorrector(string label)
		{
			if(label.StartsWith("Minions that"))
				return label.Substring(13, 9);
			else
				return label;
				
		}
		
		public void Show()
        {
            this.Visibility = Visibility.Visible;
        }

        public void Hide()
        {
            this.Visibility = Visibility.Hidden;
        }
	}
}
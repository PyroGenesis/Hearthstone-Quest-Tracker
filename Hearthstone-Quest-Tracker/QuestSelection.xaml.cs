﻿/*
 * Created by SharpDevelop.
 * User: Burhanuddin
 * Date: 22-12-2017
 * Time: 07:31 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Hearthstone_Quest_Tracker
{
	/// <summary>
	/// A trigger-happy set of dropdowns that is my GUI for choosing quests based on categories
	/// May become obsolete once I learn to read Achievements.Log
	/// 
	/// I started by using OnSelection but since I reset some fields it led to a cascade of OnSelections firing which were causing a mess
	/// So I used DropDownClosed which is much better
	/// A set of two methods: loadComboQuestData() and comboItemsGenerator() are used to repopulate the quest dropdown
	/// 
	/// </summary>
	public partial class QuestSelection : Window
	{
		// Stores actual category, quest chosen and the custom start count
		internal string category;
		internal string quest;
		internal int start_number;
		
		// The ObservableCollection is a collection that will force UI using it to update (the quest dropdown)
		// The tracker object is used for Setting quests (which I get from main tracker)
		private ObservableCollection<ComboBoxItem> quest_list;
		private QuestTracker tracker;

		// Initialize stuff, display the window, empty out category, quest and start count and set the source of quest dropdown to the ObservableCollection
		public QuestSelection(QuestTracker _tracker)
		{
			InitializeComponent();
			this.tracker = _tracker;
			this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			this.Visibility = Visibility.Visible;
			this.category = "";
			this.quest = "";
			this.start_number = 0;
			this.quest_list = new ObservableCollection<ComboBoxItem>();
			// The second comboBox source is set to the ObservableCollection so that it is dynamic and any changes are reflected
			comboQuest.ItemsSource = quest_list;
		}
		
		// Category should reset everything else so do that
		// Plus change the label of, and make quest dropdown visible, and trigger the function that loads data into it based on category
		// FIXME: An error when switching dropdowns
		void ComboCategory_DropDownClosed(object sender, EventArgs e)
		{
			// Assign content of first comboBox to category variable
			category = ((ComboBoxItem)comboCategory.SelectedItem).Content.ToString();
			// Clear out any pre-selected quests
			quest = "";
			// Populate second comboBox based on value of category, employing the ObservableCollection
			loadComboQuestData(category);
			
			// Modify the label of the second comboBox based on category selected (just aesthetics)
			if(category.Equals("Class"))
				labelQuest.Content = "Choose class:";
			else if(category.Equals("Minion"))
				labelQuest.Content = "Choose minion:";
			else if(category.Equals("Card Type"))
				labelQuest.Content = "Card type:";
			else
				labelQuest.Content = "Choose quest:";
			
			// Make the second label and comboBox visible
			labelQuest.Visibility = Visibility.Visible;
			comboQuest.Visibility = Visibility.Visible;
		}
		
		// Simply set the content of the second combo box as quest once it is closed
		void ComboQuest_DropDownClosed(object sender, EventArgs e)
		{
			quest = ((ComboBoxItem)comboQuest.SelectedItem).Content.ToString();
		}
		
		// Automatically empty out custom start count (when 0) for better experience
		void Startno_GotFocus(object sender, RoutedEventArgs e)
		{
			startno.Text = startno.Text == "0" ? string.Empty : startno.Text;
		}
		
		// If every data is OK, try to add quest
		// If not display error
		// If failure display error (>3 quests not possible)
		// TODO: Clear everyhing after success or failure
		void BtnAddQuest_Click(object sender, RoutedEventArgs e)
		{
			String strstartno = startno.Text;
			// String.IsNullOrEmpty() seems a lot cleaner than .Equals("")
			if(String.IsNullOrEmpty(category) || String.IsNullOrEmpty(quest))
			{
				MessageBox.Show("Please fill up all required fields", "Failed to add quest", MessageBoxButton.OK, MessageBoxImage.Stop);
			}
			// Checks for the validity of start count
			else if (!int.TryParse(strstartno, out start_number))
			{
				MessageBox.Show("Start number is not a valid number", "Failed to add quest", MessageBoxButton.OK, MessageBoxImage.Stop);
			}
			else if (start_number<0 || start_number>999)
			{
				MessageBox.Show("Start number should be in range 0-1000", "Failed to add quest", MessageBoxButton.OK, MessageBoxImage.Stop);
			}
			// If all inputs are valid
			else
			{
				// Attempt to set the quest in tracker
				bool status = tracker.SetQuest(quest, start_number);
				if(status)
				{
					MessageBox.Show("Quest for "+quest+" added!", "Quest added successfully", MessageBoxButton.OK, MessageBoxImage.Information);
				}
				else
				{
					MessageBox.Show("You can only have 3 quests active at one time", "Failed to add quest", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				}
			}
		}
		
		// Click trigger for the "Clear All" button
		// Puts out a confirm dialog then clears the quest list by executing a function using the tracker object
		void ClearQuests_Click(object sender, RoutedEventArgs e)
		{
			MessageBoxResult confirmClear = MessageBox.Show("Do you want to clear all tracked quests?", "Are you sure", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
			
			if(confirmClear == MessageBoxResult.Yes)
			{
				tracker.clearQuests();
			}
		}
		
		// This puts the items in an string array form so that editing and adding is easier
		// Triggers comboItemsGenerator() with the appropriate array
		// Divides quests into 5 categories: Class, Minion, ManaCost, CardType and Misc
		// TODO: Make quests a global resource
		private void loadComboQuestData(string _category)
		{
			string[] classes =
			{
				"Druid",
				"Hunter",
				"Mage",
				"Paladin",
				"Priest",
				"Shaman",
				"Rogue",
				"Warlock",
				"Warrior"
			};
			string[] minions =
			{
				"Beast",
				"Demon",
				"Murloc",
				"Pirate",
				"Elemental",
				"Battlecry",
				"Deathrattle",
				"Divine Shield",
				"Enrage",
				"Taunt"
			};
			string[] manaCosts = {
				"Minions that cost <= 2",
				"Minions that cost >= 5"
			};
			string[] cardtypes =
			{
				"Spell",
				"Weapon",
				"Combo",
				"Secret"
			};
			string[] others =
			{
				"Hero Power",
				"Take Turns"
			};
			
			if(_category.Equals("Class"))
			{
				comboItemsGenerator(classes);
			}
			else if(_category.Equals("Minion"))
			{
				comboItemsGenerator(minions);
			}
			else if(_category.Equals("Mana Cost"))
			{
				comboItemsGenerator(manaCosts);
			}
			else if(_category.Equals("Card Type"))
			{
				comboItemsGenerator(cardtypes);
			}
			else if(_category.Equals("Misc"))
			{
				comboItemsGenerator(others);
			}
		}
		
		// Clear the ObservableCollection, convert string array to ComboBoxItem/s and add it to it. Boom, quest dropdown updated.
		private void comboItemsGenerator(String[] quests)
		{
			quest_list.Clear();
			
			foreach (var q in quests)
			{
				ComboBoxItem c = new ComboBoxItem();
				c.Content = q;
				quest_list.Add(c);
			}
		}
	}
}
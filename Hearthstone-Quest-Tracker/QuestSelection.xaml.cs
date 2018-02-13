/*
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
	/// TODO: Add support for custom count
	/// </summary>
	public partial class QuestSelection : Window
	{
		// Stores actual cateegory and quest chosen
		internal string category;
		internal string quest;
		
		// The ObservableCollection is a collection that will force UI using it to update (the quest dropdown)
		// The tracker object is used for Setting quests (which I get from main tracker)
		private ObservableCollection<ComboBoxItem> quest_list;
		private QuestTracker tracker;

		// Initialize stuff, display the window, empty out category and quest and set the source of quest dropdown to the ObservableCollection
		public QuestSelection(QuestTracker _tracker)
		{
			InitializeComponent();
			this.tracker = _tracker;
			this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			this.Visibility = Visibility.Visible;
			this.category = "";
			this.quest = "";
			this.quest_list = new ObservableCollection<ComboBoxItem>();
			comboQuest.ItemsSource = quest_list;
		}
		
		// Category should reset everything else so do that
		// Plus change the label of, and make quest dropdown visible, and trigger the function that loads data into it based on category
		// FIXME: An error when switching dropdowns
		void ComboCategory_DropDownClosed(object sender, EventArgs e)
		{
			category = ((ComboBoxItem)comboCategory.SelectedItem).Content.ToString();
			quest = "";
			loadComboQuestData(category);
			
			if(category.Equals("Class"))
				labelQuest.Content = "Choose class:";
			else if(category.Equals("Minion"))
				labelQuest.Content = "Choose minion:";
			else if(category.Equals("Card Type"))
				labelQuest.Content = "Card type:";
			else if(category.Equals("Misc"))
				labelQuest.Content = "Choose quest:";
			labelQuest.Visibility = Visibility.Visible;
			comboQuest.Visibility = Visibility.Visible;
		}
		
		void ComboQuest_DropDownClosed(object sender, EventArgs e)
		{
			quest = ((ComboBoxItem)comboQuest.SelectedItem).Content.ToString();
		}
		
		// If every data is OK, try to add quest
		// If not display error
		// If failure display error (>3 quests not possible)
		// TODO: Clear everyhing after success or failure
		void BtnAddQuest_Click(object sender, RoutedEventArgs e)
		{
			// String.IsNullOrEmpty() seems a lot cleaner than .Equals("") 
			if(String.IsNullOrEmpty(category) || String.IsNullOrEmpty(quest))
			{
				MessageBox.Show("Please fill up all required fields", "Failed to add quest", MessageBoxButton.OK, MessageBoxImage.Stop);
			}
			else
			{
				bool status = tracker.SetQuest(quest);
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
		
		// This puts the items in an string array form so that editing and adding is easier
		// Triggers comboItemsGenerator() with the appropriate array
		// Divides quests into 4 categories: Class, Minion, CardType and Misc
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
				"Elemental"
			};
			string[] cardtypes =
			{
				"Spell",
				"Weapon"
			};
			string[] others =
			{
				"Hero Power",
				"Minions that cost <= 2",
				"Minions that cost >= 5"
			};
			
			if(_category.Equals("Class"))
			{
				comboItemsGenerator(classes);
			}
			else if(_category.Equals("Minion"))
			{
				comboItemsGenerator(minions);
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
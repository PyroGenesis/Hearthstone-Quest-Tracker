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
	public partial class QuestSelection : Window
	{
		internal string category;
		internal string quest;
		private ObservableCollection<ComboBoxItem> quest_list;
		private QuestTracker tracker;

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
		
		void ComboCategory_DropDownClosed(object sender, EventArgs e)
		{
			category = ((ComboBoxItem)comboCategory.SelectedItem).Content.ToString();
			quest = "";
			loadComboQuestData(category);
			
			if(category.Equals("Class"))
				labelQuest.Content = "Choose class:";
			else if(category.Equals("Other"))
				labelQuest.Content = "Choose quest:";
			labelQuest.Visibility = Visibility.Visible;
			comboQuest.Visibility = Visibility.Visible;
		}
		
		void ComboQuest_DropDownClosed(object sender, EventArgs e)
		{
			quest = ((ComboBoxItem)comboQuest.SelectedItem).Content.ToString();
		}
		
		void BtnAddQuest_Click(object sender, RoutedEventArgs e)
		{
			if(category.Equals("") || quest.Equals(""))
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
			string[] others =
			{
				"Hero Power"
			};
			
			if(_category.Equals("Class"))
			{
				comboItemsGenerator(classes);
			}
			else if(_category.Equals("Other"))
			{
				comboItemsGenerator(others);
			}
		}
		
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
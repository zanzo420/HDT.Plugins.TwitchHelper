﻿using System;
using System.IO;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Utility.Logging;

namespace HDT.Plugins.TwitchHelper
{
    [Serializable]
    public class PluginSettings : Object
    {
        public event EventHandler settingUpdated;

        public bool twitchFile { get; set; }
        public string twitchFileText { get; set; }
        public string twitchFilename { get; set; }
        public string deckImagesLocation { get; set; }
        public string currentDeckFilename { get; set; }
        public bool changeImage { get; set; }        

        
        public void LoadSettings(string filePath)
        {
            PluginSettings settings;
            if (File.Exists(filePath))
            {
                settings = XmlManager<PluginSettings>.Load(filePath);
                this.twitchFile = settings.twitchFile;
                this.twitchFilename = settings.twitchFilename;
                this.twitchFileText = settings.twitchFileText;
                this.changeImage = settings.changeImage;
                this.currentDeckFilename = settings.currentDeckFilename;
                this.deckImagesLocation = settings.deckImagesLocation;
            }
            else
            {
                this.twitchFile = true;
                this.twitchFilename = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\CurrentDeckStats.txt";
                this.twitchFileText = "{DeckName}: {WinLoss} ({WinLossPercent})";
                this.currentDeckFilename = "current-deck.png";
                this.deckImagesLocation = "";                
                this.changeImage = false;

                SaveSettings(filePath);
            }           
        }

        public void SaveSettings(string filePath)
        {
            try
            {
                if (!Directory.Exists(TwitchHelperPlugin._PluginDataDir))
                {
                    Directory.CreateDirectory(TwitchHelperPlugin._PluginDataDir);
                }

                if (Directory.Exists(TwitchHelperPlugin._PluginDataDir))
                {
                    XmlManager<PluginSettings>.Save(filePath, this);
                    OnsettingUpdated(new EventArgs());
                }

            }
            catch(Exception ex)
            {
                Log.Error(ex);
            }
        }

        protected virtual void OnsettingUpdated(EventArgs e)
        {
            if (settingUpdated != null)
                settingUpdated(this, e);
        }
    }
}
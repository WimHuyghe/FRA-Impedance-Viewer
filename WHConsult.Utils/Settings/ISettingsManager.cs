//-----------------------------------------------------------------------------
//
//  ISettingsManger.cs
//
//  Part of Utillities Library from Wim Huyghe
//
//  Copyright (C) 2012 Wim Huyghe
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
//  Web:    http://www.WHConsult.be
//  Email:  SpeakerReport@WHConsult.be
//
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WHConsult.Utils.Events;

namespace WHConsult.Utils.Settings
{
    public interface ISettingsManager
    {
        // Get Values
         string GetStringValue(string settingName, string defaultValue);
         bool GetBooleanValue(string settingName, bool defaultValue);
         byte GetByteValue(string settingName, byte defaultValue);
         int GetIntValue(string settingName, int defaultValue);
         double GetDoubleValue(string settingName, double defaultValue);

        // Set Values
         void SetStringValue(string settingName, string value);
         void SetBooleanValue(string settingName, bool value);
         void SetByteValue(string settingName, byte value);
         void SetIntValue(string settingName, int value);
         void SetDoubleValue(string settingName, double value);

        // Save
         void SaveValues();

        // Events
         event EventHandler<SettingChangedEventArgs> NewFileLoaded;
    }
}

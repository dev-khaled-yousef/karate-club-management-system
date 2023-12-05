﻿using KarateClub_Business;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KarateClub.Global_Classes
{
    internal static class clsGlobal
    {
        public static clsUser CurrentUser;

        public static bool RememberUsernameAndPassword(string Username, string Password)
        {
            string keyPath = @"HKEY_CURRENT_USER\SOFTWARE\KarateClub";

            string UsernameName = "Username";
            string UsernameData = Username;

            string PasswordName = "Password";
            string PasswordData = Password;

            try
            {
                // Write the value to the Registry
                Registry.SetValue(keyPath, UsernameName, UsernameData, RegistryValueKind.String);
                Registry.SetValue(keyPath, PasswordName, PasswordData, RegistryValueKind.String);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An Error occurred: {ex.Message}");
                return false;
            }
        }

        public static bool RemoveStoredCredential()
        {
            string keyPath = @"SOFTWARE\KarateClub";

            string UsernameName = "Username";
            string PasswordName = "Password";

            try
            {
                // Create or open the registry key
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(keyPath))
                {
                    // Check if the key exists before attempting to delete values
                    if (key == null)
                    {
                        MessageBox.Show($"Registry key not found: {keyPath}");
                        return false;
                    }

                    // Remove only the data, leaving the value name intact
                    key.DeleteValue(UsernameName, false);
                    key.DeleteValue(PasswordName, false);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public static bool GetStoredCredential(ref string Username, ref string Password)
        {
            string keyPath = @"HKEY_CURRENT_USER\SOFTWARE\KarateClub";

            string UsernameName = "Username";
            string PasswordName = "Password";

            try
            {
                // Read the value from the Registry
                Username = Registry.GetValue(keyPath, UsernameName, null) as string;
                Password = Registry.GetValue(keyPath, PasswordName, null) as string;

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An Error occurred: {ex.Message}");
                return false;
            }
        }

        public static bool CheckAccessDenied(clsUser.enPermissions enPermissions)
        {
            if (CurrentUser.Permissions == (int)clsUser.enPermissions.All)
                return true;


            if (((int)enPermissions & CurrentUser.Permissions) == (int)enPermissions)
                return true;

            else
                return false;

        }
    }
}

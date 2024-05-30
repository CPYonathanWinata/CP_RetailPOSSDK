/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows.Forms;
using LSRetailPosis;

namespace Microsoft.Dynamics.Retail.Pos.TaskRecorder.Common
{
    /// <summary>
    /// Internal utils
    /// </summary>
    internal static class Utils
    {
        #region fields
        internal const string ScreenshotsWorkingFolder = "ScreenshotsWorkspace";
        #endregion

        #region internal methods

        /// <summary>
        /// Gets the screenshot.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="recordId">The record identifier.</param>
        /// <param name="folderPath">The folder path.</param>
        /// <returns>Screenshot.</returns>
        public static Bitmap GetScreenshot(Control control, Guid recordId, string folderPath)
        {
            Bitmap screenShot = null;
            if ((control != null) && (control.TopLevelControl != null) && (!string.IsNullOrWhiteSpace(folderPath)))
            {
                Control topControl = control.TopLevelControl;
                Rectangle bounds = topControl.Bounds;

                using (screenShot = new Bitmap(topControl.Width, topControl.Height))
                {
                    using (Graphics g = Graphics.FromImage(screenShot))
                    {
                        g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                    }

                    string fullScreenShotsWorkingFolder = Path.Combine(folderPath, ScreenshotsWorkingFolder);

                    if (!Directory.Exists(fullScreenShotsWorkingFolder))
                    {
                        Directory.CreateDirectory(fullScreenShotsWorkingFolder);
                    }


                    string currentScreenshot = Path.Combine(fullScreenShotsWorkingFolder, string.Format("{0}.png", recordId));
                    screenShot.Save(currentScreenshot, ImageFormat.Png);
                }
            }
            return screenShot;
        }

        /// <summary>
        /// Executes action if the specified key is found in the dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="action">The action.</param>
        public static void ExecuteIfFound<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Action<TValue> action)
        {
            TValue value;
            if ((dictionary != null) && (dictionary.TryGetValue(key, out value)))
            {
                action(value);
            }
        }

        /// <summary>
        /// Gets all the enum values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Enumerated enum values.</returns>
        public static T[] GetEnumValues<T>()
        {
            return Enum.GetValues(typeof(T)).OfType<T>().ToArray();
        }

        /// <summary>
        /// Performs action on each item in the specified collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The collection.</param>
        /// <param name="action">The action.</param>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    action(item);
                }
            }
        }

        /// <summary>
        /// Creates the binding.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="dataSource">The data source.</param>
        /// <param name="dataMember">The data member.</param>
        /// <returns>Created binding.</returns>
        public static Binding AddBinding(this IBindableComponent bindableComponent, string propertyName, object dataSource, string dataMember)
        {
            Binding binding = new Binding(propertyName, dataSource, dataMember, true)
            {
                DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged,
                ControlUpdateMode = ControlUpdateMode.OnPropertyChanged
            };
            if (bindableComponent != null)
            {
                bindableComponent.DataBindings.Add(binding);
            }
            return binding;
        }

        /// <summary>
        /// Gets concrete object from json.
        /// </summary>
        /// <typeparam name="T">Type of concrete object</typeparam>
        /// <param name="json">The json.</param>
        /// <returns>Concrete object from json. Returns null if cannot convert to concrete type.</returns>
        public static T GetFromJson<T>(string json)
        {
            T returnObject = default(T);

            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                returnObject = (T)deserializer.ReadObject(stream);
            }

            return returnObject;
        }

        /// <summary>
        /// Gets json converted from concrete object.
        /// </summary>
        /// <typeparam name="T">Type of concrete object</typeparam>
        /// <param name="obj">The concrete object.</param>
        /// <returns>Json converted from the concrete object. Returns null if cannot convert.</returns>
        public static string GetJson<T>(T obj)
        {
            string returnJson = null;

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, obj);
                returnJson = Encoding.Default.GetString(memoryStream.ToArray());
            }

            return returnJson;
        }


        /// <summary>
        /// Converts the specified source from target using json serialization/deserialization. Use this if you have same properties in two types, but no common base type.
        /// All matching properties in target will have original values and others will have null values.
        /// </summary>
        /// <typeparam name="TSourceType">The type of the source object.</typeparam>
        /// <typeparam name="TTargetType">The type of the target object.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>Converted object.</returns>
        public static TTargetType Convert<TSourceType, TTargetType>(this TSourceType source)
        {
            string sourceJson = GetJson(source);
            TTargetType targetObj = GetFromJson<TTargetType>(sourceJson);
            return targetObj;
        }

        /// <summary>
        /// Returns the translation of text that is shown to the user.
        /// </summary>
        /// <param name="textId">The unique id of the text to be translated.</param>
        /// <returns>
        /// The translated text.
        /// </returns>
        public static string Translate(this int textId)
        {
            return ApplicationLocalizer.Language.Translate(textId);
        }

        /// <summary>
        /// Returns a hash code unique for the combination of the specified two strings. Order of strings does not matter.
        /// </summary>
        /// <param name="first">The first string.</param>
        /// <param name="second">The second string.</param>
        /// <returns>
        /// A hash code for the combination of the specified two strings, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public static int GetHashCode(string first, string second)
        {
            return first.GetHashCode() ^ second.GetHashCode();
        }

        #endregion
    }
}

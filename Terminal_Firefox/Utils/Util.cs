﻿using System;
using System.IO;
using Gecko;
using NLog;

namespace Terminal_Firefox.Utils {

    
    /// <summary>
    /// Типы команд
    /// </summary>
    public enum CommandTypes {
        Payment = 0, Link = 1, Settings
    }


    public static class Util {

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Типы сервисов
        /// 1. Главные кнопки
        /// 2. Кнонки быстрого набора
        /// </summary>
        public enum ServiceTypes {
            MainService = 0,
            Service = 1
        }

        
        public static string GetSubServices(short serviceId, ServiceTypes serviceType) {
            try {
                string path = (serviceType == ServiceTypes.MainService)
                                  ? "/html/js/data/main_buttons/"
                                  : "/html/js/data/services/";
                return File.ReadAllText(Directory.GetCurrentDirectory() + path + serviceId + ".js");
            } catch (Exception ex) {
                Log.Error(String.Format("Service file with id {0} does not found", serviceId), ex);
            }
            return "";
        }

        public static void AddJSToDom(GeckoWebBrowser browser, string textContent) {
            try {
                var innerHtml = browser.Document.CreateElement("script");
                innerHtml.TextContent = textContent;
                browser.Document.Head.AppendChild(innerHtml);

            } catch (Exception exception) {
                Log.Error(exception);
                Log.Error("Невозможно добавить script к webbrowser");
            }
        }

        public static void NavigateTo(GeckoWebBrowser browser, CurrentWindow window) {
            try {
                string location = @"\html\index.html";
                switch (window) {
                    case CurrentWindow.Dependent:
                        location = @"\html\dependent.html";
                        break;
                    case CurrentWindow.EnterNumber:
                        location = @"\html\enter_number.html";
                        break;
                    case CurrentWindow.Pay:
                        location = @"\html\pay.html";
                        break;
                    case CurrentWindow.Encashment:
                        location = @"\html\encashment.html";
                        break;
                    case CurrentWindow.MakeEncashment:
                        location = @"\html\make_encashment.html";
                        break;
                    case CurrentWindow.BlockTerminal:
                        location = @"\html\terminal_blocked.html";
                        break;
                }
                browser.Navigate(Directory.GetCurrentDirectory() + location);

            } catch (Exception exception) {
                Log.Error("Cannot navigate to the page");
            }
        }

        public static void AppendImageElement(GeckoWebBrowser browser, string elementId, int path) {
            try {
                var innerHtml = browser.Document.CreateElement("img");
                innerHtml.SetAttribute("src", "images/service_logos/" + path + "_m.png");
                innerHtml.SetAttribute("width", "100%");
                innerHtml.SetAttribute("height", "100%");
                browser.Document.GetElementById(elementId).AppendChild(innerHtml);
            } catch (Exception exception) {
                Log.Error("Немогу добавить картинку");
            }
        }

        public static void AppendText(GeckoWebBrowser browser, string commission, string elementId) {
            try {
            browser.Document.GetElementById(elementId).TextContent = commission;

            } catch (Exception exception) {
                Log.Error(exception);
            }
        }
		
		public static string GetCurrentTimestamp() {
            return ((Int32) (DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
        }
    }
}

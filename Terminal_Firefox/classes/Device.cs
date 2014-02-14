﻿using System;
using System.Data.SqlServerCe;
using NLog;

namespace Terminal_Firefox.classes {
    class Device {

        public enum Devices {
            CashCode, Printer, CashSum, CashCount, Terminal
        }


        private static readonly Logger Log = LogManager.GetCurrentClassLogger();


        public static void SetDeviceState(Devices device, string state) {
            using (SqlCeConnection connection = new SqlCeConnection()) {
                connection.ConnectionString = DBWrapper.ConnectionString;
                using (SqlCeCommand command = new SqlCeCommand()) {
                    try {
                        command.Connection = connection;
                        command.CommandText = "update perif_state set perif_state = @state where perif = @device";
                        command.Parameters.Add("@state", state);
                        command.Parameters.Add("@device", device);
                        connection.Open();
                        command.ExecuteNonQuery();
                    } catch (Exception exception) {
                        Log.Error("Невозможно обновить статус устройства", exception);
                    } finally { command.Parameters.Clear(); }
                }
            }
        }
    }
}
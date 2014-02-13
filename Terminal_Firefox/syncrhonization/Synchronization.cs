﻿using System.Threading;
using Terminal_Firefox.Utils;
using Terminal_Firefox.classes;

namespace Terminal_Firefox.syncrhonization {
    public class Synchronization {
        private Payment _payment;
        private readonly Communication _communication = new Communication();
        private CommandTypes _lastCommand;

        public void Synchronize() {

            _communication.Autorize();
            
            while (true) {
                Thread.Sleep(15000);
                _payment = Payment.GetSingle();

                string preparedCommand = Command.Prepare(CommandTypes.Link, new Link());
                _communication.SSend(preparedCommand);
                _lastCommand = CommandTypes.Link;

                if (_payment != null) {
                    preparedCommand = Command.Prepare(CommandTypes.Payment, _payment);
                    Command.HandleAnswer(_communication.SSend(preparedCommand));
                    _lastCommand = CommandTypes.Payment;
                }
            }
        }
    }
}

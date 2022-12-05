﻿using Mach3_netframework.MACH3.InpOut32x64;
using Mach3_netframework.MACH3.Interface;
using System.Collections.Generic;

namespace Mach3_netframework.MACH3
{
    public class Mach3
    {
        public delegate bool TurnDelegate();
        public delegate string Log(string msg);

        public Log Logs { get; set; }
        public bool IsTurn { get; set; }
        public bool GetTurn() => IsTurn;
        
        private readonly InpOut inpOut = new InpOut();

        public Mach3Toggle Spindle { get; }
        public Mach3Toggle Clamp { get; }

        public Mach3AxisMotor X { get; } = new Mach3AxisMotor("X", new short[2, 2] { { 5, 0 }, { 15, 10 } });
        public Mach3AxisMotor Y { get; } = new Mach3AxisMotor("Y", new short[2, 2] { { 16, 0 }, { 48, 32 } });
        public Mach3AxisMotor Z { get; } = new Mach3AxisMotor("Z", new short[2, 2] { { 192, 128 }, { 64, 0 } });

        // public Mach3AxisMotor A { get; } = new Mach3AxisMotor("A", new short[2, 2] { { ??, ?? }, { ??, ?? } });
        // public Mach3AxisMotor B { get; } = new Mach3AxisMotor("B", new short[2, 2] { { ??, ?? }, { ??, ?? } });

        Mach3Sensor Sensor1 { get; } = new Mach3Sensor(14, 1);
        Mach3Sensor Sensor2 { get; } = new Mach3Sensor(6, 1);
        Mach3Sensor Sensor3 { get; } = new Mach3Sensor(2, 1);
        Mach3Sensor Sensor4 { get; } = new Mach3Sensor(0, 1);
        Mach3Sensor Sensor5 { get; } = new Mach3Sensor(0, 0);


        public Mach3()
        {
            this.Spindle = new Mach3Toggle(890, 8);
            SubribeObj(this.Spindle);
            this.Spindle.Off();
            Logs?.Invoke("Spindle load and stop");

            this.Clamp = new Mach3Toggle(890, 2);
            SubribeObj(this.Clamp);
            this.Clamp.Off();
            Logs?.Invoke("Clamp load and stop");

            SubribeObj(this.X);
            Logs?.Invoke("Make X axis");
            SubribeObj(this.Y);
            Logs?.Invoke("Make Y axis");
            SubribeObj(this.Z);
            Logs?.Invoke("Make Z axis");


        }

        public void SubribeObj(object obj)
        {
            if (obj is ITurnElement turnElement)
            {
                turnElement.TurnOnAll = GetTurn;
            }
            if (obj is IInpOutElement outElement)
            {
                outElement.Inp = inpOut.Inp;
                outElement.Out = inpOut.Out;
            }
        }

    }
}

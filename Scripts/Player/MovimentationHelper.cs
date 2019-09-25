// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PolarisCore
{
    public class MovimentationHelper
    {
        private static float tempV = 0f;
        private static float tempH = 0f;

        //SIMULACAO DOS EIXOS VERTICAIS E HORIZONTAIS DE INPUT
        private static float verticalAxis = 0f;
        private static float verticalWalkAcceleration = .6f;//ACELERACAO AO SE MOVER
        private static float verticalWalkCounterAcceleration = verticalWalkAcceleration * 1f;//DESACELERACAO QUANDO QUISER MOVER CONTRA O MOVIMENTO ATUAL

        private static float horizontalAxis = 0f;
        private static float horizontalWalkAcceleration = .6f;//ACELERACAO AO SE MOVER
        private static float horizontalWalkCounterAcceleration = horizontalWalkAcceleration * 1f;//DESACELERACAO QUANDO QUISER MOVER CONTRA O MOVIMENTO ATUAL

        public static void Axify(int h, int v)
        {
            AxisController(v, tempV, out verticalAxis, verticalWalkAcceleration, verticalWalkCounterAcceleration);
            AxisController(h, tempH, out horizontalAxis, horizontalWalkAcceleration, horizontalWalkCounterAcceleration);
            tempV = verticalAxis;
            tempH = horizontalAxis;
        }

        public static void GetAxis(out float h, out float v)
        {
            h = horizontalAxis;
            v = verticalAxis;
        }

        private static void AxisController(int rawAxis, float axis, out float outAxis, float accel, float counterAccel)
        {
            outAxis = 0f;
            if (rawAxis > 0)
            {
                if (axis < 0f)
                {
                    outAxis = axis + counterAccel;
                }
                else if (axis < 1f)
                {
                    outAxis = axis + accel;
                }
            }
            else if (rawAxis < 0)
            {
                if (axis > 0f)
                {
                    outAxis = axis - counterAccel;
                }
                else if (axis > -1f)
                {
                    outAxis = axis - accel;
                }
            }
            else
            {
                if (axis > 0.1f)
                {
                    outAxis = axis - accel;
                }
                else if (axis < -0.1f)
                {
                    outAxis = axis + accel;
                }
                else
                {
                    outAxis = 0f;
                }
            }
        }
    }
}

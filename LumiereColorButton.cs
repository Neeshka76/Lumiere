using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumiere
{
    public class LumiereColorButton
    {
        public float r;
        public float g;
        public float b;
        public LumiereColorButton(LumiereColor.LightPositiveColors lightPositive, bool positiveColor = true)
        {
            switch (lightPositive)
            {
                case LumiereColor.LightPositiveColors.White:
                    r = 255f;
                    g = 255f;
                    b = 255f;
                    break;
                case LumiereColor.LightPositiveColors.Red:
                    r = 255f;
                    g = 0f;
                    b = 0f;
                    break;
                case LumiereColor.LightPositiveColors.DarkRed:
                    r = 127f;
                    g = 0f;
                    b = 0f;
                    break;
                case LumiereColor.LightPositiveColors.Green:
                    r = 0f;
                    g = 255f;
                    b = 0f;
                    break;
                case LumiereColor.LightPositiveColors.DarkGreen:
                    r = 0f;
                    g = 127f;
                    b = 0f;
                    break;
                case LumiereColor.LightPositiveColors.Blue:
                    r = 0f;
                    g = 0f;
                    b = 255f;
                    break;
                case LumiereColor.LightPositiveColors.DarkBlue:
                    r = 0f;
                    g = 0f;
                    b = 127f;
                    break;
                case LumiereColor.LightPositiveColors.Cyan:
                    r = 0f;
                    g = 255f;
                    b = 255f;
                    break;
                case LumiereColor.LightPositiveColors.DarkCyan:
                    r = 0f;
                    g = 127f;
                    b = 127f;
                    break;
                case LumiereColor.LightPositiveColors.LightCyan:
                    r = 127f;
                    g = 255f;
                    b = 255f;
                    break;
                case LumiereColor.LightPositiveColors.Turquoise:
                    r = 0f;
                    g = 127f;
                    b = 255f;
                    break;
                case LumiereColor.LightPositiveColors.Orange:
                    r = 255f;
                    g = 127f;
                    b = 0f;
                    break;
                case LumiereColor.LightPositiveColors.DarkOrange:
                    r = 255f;
                    g = 64f;
                    b = 0f;
                    break;
                case LumiereColor.LightPositiveColors.Yellow:
                    r = 255f;
                    g = 255f;
                    b = 0f;
                    break;
                case LumiereColor.LightPositiveColors.Gold:
                    r = 255f;
                    g = 215f;
                    b = 0f;
                    break;
                case LumiereColor.LightPositiveColors.Violet:
                    r = 255f;
                    g = 0f;
                    b = 127f;
                    break;
                case LumiereColor.LightPositiveColors.DarkViolet:
                    r = 127f;
                    g = 0f;
                    b = 127f;
                    break;
            }
            if(!positiveColor)
            {
                r = -r;
                g = -g;
                b = -b;
            }
        }
        public float RValue()
        {
            return r;
        }
        public float GValue()
        {
            return g;
        }
        public float BValue()
        {
            return b;
        }
    }
}

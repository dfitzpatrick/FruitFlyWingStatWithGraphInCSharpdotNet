using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FruitFlyWingStatWithGraphInCSharpdotNet
{
    public static class Global
    {
        //Scaling
        public static bool AUTO_SCALING = false;
        public static double X_AXIS_MINIMUM = 0;
        public static double X_AXIS_MAXIMUM = 2.5;
        public static double X_AXIS_INTERVAL = 0.5;
        public static double Y_AXIS_MINIMUM = 0;
        public static double Y_AXIS_MAXIMUM = 2;
        public static double Y_AXIS_INTERVAL = 0.5;
        //Colors
        public static Color LINE_COLOR = Color.Red;
        public static Color POINT_COLOR = Color.Blue;
        //Widths
        public static int LINE_WIDTH = 2;
        public static int POINT_SIZE = 10;
        //Data Labels
        public static bool SHOW_DATA_LABELS = true;
        public static bool SHOW_DATA_LABELS_AS_TOOLTIP = true;
        public static Color DATA_LABEL_COLOR = Color.Blue;
        //Axis Titles
        public static bool SHOW_X_AXIS_TITLE = false;
        public static bool SHOW_Y_AXIS_TITLE = false;
        public static string X_AXIS_TITLE = "X-Axis";
        public static string Y_AXIS_TITLE = "Y-Axis";


    }
}

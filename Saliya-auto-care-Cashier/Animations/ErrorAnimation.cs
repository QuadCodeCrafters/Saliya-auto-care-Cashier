using System.Windows.Media.Animation;
using System;

namespace Saliya_auto_care_Cashier.Animations
{
    internal static class ErrorAnimation
    {
        public static readonly DoubleAnimation animation = new DoubleAnimation
        {
            From = 0,
            To = 10,
            Duration = TimeSpan.FromMilliseconds(50),
            AutoReverse = true,
            RepeatBehavior = new RepeatBehavior(5) // Shake 5 times
        };
    }
}

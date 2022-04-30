using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Курсовая.ProgrammInterface;

namespace Курсовая.Setting
{
    public class WorkWithBusList : IWorkWithBusList
    {
        public void BucketListBoxFirst(object sender, MouseButtonEventArgs e)
        {
            var listBox = sender as ListBox;
            if (null == listBox)
                return;

            var point = e.GetPosition((UIElement)sender);

            VisualTreeHelper.HitTest(listBox, null, (hitTestResult) =>
            {
                var uiElement = hitTestResult.VisualHit as UIElement;

                while (null != uiElement)
                {
                    var listBoxItem = uiElement as ListBoxItem;
                    if (null != listBoxItem)
                    {
                        listBoxItem.IsSelected = true;
                        return HitTestResultBehavior.Stop;
                    }

                    uiElement = VisualTreeHelper.GetParent(uiElement) as UIElement;
                }

                return HitTestResultBehavior.Continue;
            }, new PointHitTestParameters(point));
        }

        public void BucketListBoxSecond(object sender, MouseButtonEventArgs e)
        {
            var listBox = sender as ListBox;
            if (null == listBox)
                return;

            var point = e.GetPosition((UIElement)sender);

            VisualTreeHelper.HitTest(listBox, null, (hitTestResult) =>
            {
                var uiElement = hitTestResult.VisualHit as UIElement;
                ListBoxItem firstLisBoxItem = null;
                ListBoxItem SecondtLisBoxItem = null;

                while (null != uiElement)
                {
                    if (uiElement == uiElement as ListBoxItem)
                    {
                        if (firstLisBoxItem == null)
                            firstLisBoxItem = uiElement as ListBoxItem;
                        else
                            SecondtLisBoxItem = uiElement as ListBoxItem;
                    }
                    if (firstLisBoxItem != null && SecondtLisBoxItem != null)
                    {
                        SecondtLisBoxItem.IsSelected = true;
                        firstLisBoxItem.IsSelected = true;
                        return HitTestResultBehavior.Stop;
                    }

                    uiElement = VisualTreeHelper.GetParent(uiElement) as UIElement;
                }

                return HitTestResultBehavior.Continue;
            }, new PointHitTestParameters(point));
        }
    }
}

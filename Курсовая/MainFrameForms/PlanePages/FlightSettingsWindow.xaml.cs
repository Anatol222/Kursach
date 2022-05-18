using System;
using System.Windows;
using System.Windows.Input;

namespace Курсовая.MainFrameForms.SityBusPages
{
    public partial class FlightSettingsWindow : Window
    {
        public FlightSettingsWindow()
        {
            InitializeComponent();
            DataContext = this;
            OlderTb.Text = Convert.ToString(OlderCount);
        }

        private int OlderCount = 1;
        private int KidCount = 0;
        private int BabyCount = 0;
        private int HandLugCount = 0;
        private int LuggageCount = 0;

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)=>
            DragMove();

        private void CancelBtn_Click(object sender, RoutedEventArgs e)=>
            this.Close();

        private void IsReadyBtn_Click(object sender, RoutedEventArgs e)
        {
            PlanePage.PeopleCount = OlderCount + KidCount + BabyCount;
            PlanePage.AllLuggageCount = HandLugCount + LuggageCount;
            Close();
        }

        private void OlderBtnMinus_Click(object sender, RoutedEventArgs e)
        {
            if (OlderCount >= 2 && OlderCount <= 8 - KidCount - BabyCount)
            {
                OlderBtnPlus.IsEnabled = true;
                BabyBtnPlus.IsEnabled = true;
                KidBtnPlus.IsEnabled = true;
                OlderCount -= 1;
                OlderTb.Text = Convert.ToString(OlderCount);
                if (OlderCount == 1)
                    OlderBtnMinus.IsEnabled = false;
                if (LuggageCount > OlderCount * 4)
                {
                    LuggageCount = OlderCount * 4;
                    LugTb.Text = Convert.ToString(LuggageCount);
                    LugBtnplus.IsEnabled = false;
                }
                if (HandLugCount> OlderCount*2+KidCount)
                {
                    HandLugCount -= 2;
                    HandLugTb.Text = Convert.ToString(HandLugCount);
                    HandLugBtnPlus.IsEnabled = false;
                }
            }
            else
            {
                OlderCount -= 1;
                OlderTb.Text = Convert.ToString(OlderCount);
                OlderBtnMinus.IsEnabled = false;
            }

        }

        private void OlderBtnPlus_Click(object sender, RoutedEventArgs e)
        {
            if (OlderCount == 8 - KidCount - BabyCount - 1)
            {
                OlderCount += 1;
                OlderTb.Text = Convert.ToString(OlderCount);
                OlderBtnPlus.IsEnabled = false;
                KidBtnPlus.IsEnabled = false;
                BabyBtnPlus.IsEnabled = false;
                OlderBtnMinus.IsEnabled = true;
                if (KidCount == 0)
                    KidBtnMinus.IsEnabled = false;
                else
                    KidBtnMinus.IsEnabled = true;
                if (BabyCount == 0)
                    BabyBtnMinus.IsEnabled = false;
                else
                    BabyBtnMinus.IsEnabled = true;
            }
            else
            {
                HandLugBtnPlus.IsEnabled = true;
                LugBtnplus.IsEnabled = true;
                OlderBtnMinus.IsEnabled = true;
                OlderCount += 1;
                OlderTb.Text = Convert.ToString(OlderCount);

            }
        }

        private void KidBtnMinus_Click(object sender, RoutedEventArgs e)
        {
            if (KidCount > 1 && KidCount <= (8 - OlderCount - BabyCount))
            {
                KidBtnPlus.IsEnabled = true;
                BabyBtnPlus.IsEnabled = true;
                OlderBtnPlus.IsEnabled = true;
                KidCount -= 1;
                KidTb.Text = Convert.ToString(KidCount);
            }
            else
            {
                KidCount -= 1;
                KidTb.Text = Convert.ToString(KidCount);
                KidBtnMinus.IsEnabled = false;
                KidBtnPlus.IsEnabled = true;
                BabyBtnPlus.IsEnabled = true;
                OlderBtnPlus.IsEnabled = true;
                if (HandLugCount > OlderCount * 2 + KidCount)
                {
                    HandLugCount -= 1;
                    HandLugTb.Text = Convert.ToString(HandLugCount);
                    HandLugBtnPlus.IsEnabled = false;
                }
            }
        }

        private void KidBtnPlus_Click(object sender, RoutedEventArgs e)
        {
            if (KidCount == (8 - OlderCount - BabyCount - 1))
            {
                KidCount += 1;
                KidTb.Text = Convert.ToString(KidCount);
                KidBtnPlus.IsEnabled = false;
                OlderBtnPlus.IsEnabled = false;
                BabyBtnPlus.IsEnabled = false;
                KidBtnMinus.IsEnabled = true;
                if (OlderCount > 1)
                    OlderBtnMinus.IsEnabled = true;
                if (OlderCount == 1)
                    OlderBtnMinus.IsEnabled = false;
                else
                    OlderBtnMinus.IsEnabled = true;
                if (BabyCount == 0)
                    BabyBtnMinus.IsEnabled = false;
                else
                    BabyBtnMinus.IsEnabled = true;
            }
            else
            {
                HandLugBtnPlus.IsEnabled = true;
                KidBtnMinus.IsEnabled = true;
                KidCount += 1;
                KidTb.Text = Convert.ToString(KidCount);

            }
        }

        private void BabyBtnMinus_Click(object sender, RoutedEventArgs e)
        {
            if (BabyCount > 1 && BabyCount <= (8 - OlderCount - KidCount))
            {
                KidBtnPlus.IsEnabled = true;
                BabyBtnPlus.IsEnabled = true;
                OlderBtnPlus.IsEnabled = true;
                BabyCount -= 1;
                BabyTb.Text = Convert.ToString(BabyCount);
            }
            else
            {
                BabyCount -= 1;
                BabyTb.Text = Convert.ToString(BabyCount);
                BabyBtnMinus.IsEnabled = false;
                KidBtnPlus.IsEnabled = true;
                BabyBtnPlus.IsEnabled = true;
                OlderBtnPlus.IsEnabled = true;
            }
        }

        private void BabyBtnPlus_Click(object sender, RoutedEventArgs e)
        {
            if (BabyCount == (8 - OlderCount - KidCount - 1))
            {
                BabyCount += 1;
                BabyTb.Text = Convert.ToString(BabyCount);
                KidBtnPlus.IsEnabled = false;
                OlderBtnPlus.IsEnabled = false;
                BabyBtnPlus.IsEnabled = false;
                BabyBtnMinus.IsEnabled = true;
                if (OlderCount == 1)
                    OlderBtnMinus.IsEnabled = false;
                else
                    OlderBtnMinus.IsEnabled = true;
                if (KidCount == 0)
                    KidBtnMinus.IsEnabled = false;
                else
                    KidBtnMinus.IsEnabled = true;
            }
            else
            {
                BabyBtnMinus.IsEnabled = true;
                BabyCount += 1;
                BabyTb.Text = Convert.ToString(BabyCount);

            }
        }

        private void LugBtnMinus_Click(object sender, RoutedEventArgs e)
        {
            if (LuggageCount > 1)
            {
                LugBtnplus.IsEnabled = true;
                LuggageCount -= 1;
                LugTb.Text = Convert.ToString(LuggageCount);
            }
            else
            {
                LugBtnMinus.IsEnabled = false;
                LuggageCount -= 1;
                LugTb.Text = Convert.ToString(LuggageCount);
            }
        }

        private void LugBtnplus_Click(object sender, RoutedEventArgs e)
        {
            if (LuggageCount < OlderCount * 4)
            {
                LuggageCount += 1;
                LugTb.Text = Convert.ToString(LuggageCount);
                if (LuggageCount == OlderCount * 4)
                    LugBtnplus.IsEnabled = false;
                LugBtnMinus.IsEnabled = true;
            }

        }

        private void HandLugBtnMinus_Click(object sender, RoutedEventArgs e)
        {
            if (HandLugCount > 1)
            {
                HandLugCount -= 1;
                HandLugTb.Text = Convert.ToString(HandLugCount);
                HandLugBtnPlus.IsEnabled = true;
            }
            else
            {
                HandLugCount -= 1;
                HandLugTb.Text = Convert.ToString(HandLugCount);
                HandLugBtnMinus.IsEnabled = false;
            }
        }

        private void HandLugBtnPlus_Click(object sender, RoutedEventArgs e)
        {
            if (LuggageCount < OlderCount * 2 + KidCount)
            {
                HandLugCount += 1;
                HandLugTb.Text = Convert.ToString(HandLugCount);
                if (HandLugCount == OlderCount * 2 + KidCount)
                    HandLugBtnPlus.IsEnabled = false;
                HandLugBtnMinus.IsEnabled = true;
            }
        }
    }
}

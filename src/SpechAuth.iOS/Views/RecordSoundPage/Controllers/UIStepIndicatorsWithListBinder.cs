using System.Linq;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using Cirrious.MvvmCross.Binding;
using System;
using System.Collections.ObjectModel;

namespace SpechAuth.iOS.Views.RecordSoundPage.Controllers
{
    public class UIStepIndicatorsWithListBinder : MvxTargetBinding
    {
        protected UIStepIndicators StepIndicators {
            get { return (UIStepIndicators)Target; }
        }
        
        public UIStepIndicatorsWithListBinder (UIStepIndicators target)
            : base (target)
        {
            
        }
//        protected override void Bind()
//        {
//            UpdateDataSource();
//            SubscribeOnPropertyChanged(Frame, "DataSource", UpdateDataSource);
//        }
//
//        void UpdateDataSource()
//        {
//            Frame.DataSource.ForEach(item =>
//                {
//                    View.AddIndicator();
//                    SubscribeOnPropertyChanged(item, "Exists", () => View.FillIndicator(Frame.DataSource.IndexOf(item)));
//                });
//        }

        #region implemented abstract members of MvxTargetBinding

        public override void SetValue (object value)
        {
            var list = (ObservableCollection<bool>)value;
            if (list == null)
                return;
            for (int i = 0; i < list.Count; i++) {                
                if (list [i])
                    StepIndicators.FillIndicator (i);
            }
                
        }

        public override Type TargetType {
            get {
                return typeof(bool);
            }
        }

        public override MvxBindingMode DefaultMode {
            get {
                return MvxBindingMode.OneWay;
            }
        }

        #endregion
    }
}


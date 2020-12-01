using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator() 
        { 
            SimpleIoc.Default.Register<MainViewModel>(); 
        }
        public MainViewModel Main => SimpleIoc.Default.GetInstance<MainViewModel>();
    }
}

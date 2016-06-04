using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoZoomTestApp.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            Main = new MainViewModel();
        }

        private MainViewModel main;

        public MainViewModel Main
        {
            get { return main; }
            set { main = value; }
        }
    }
}
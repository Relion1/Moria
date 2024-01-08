using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moria
{
    internal class Mesaj
    {
        

        // bu sınıf mesajda dosya var mı yok mu bunun için oluşturuldu.

        public string Icerik { get; set; }
            public bool DosyaVarMi { get; set; }
            

            public UserControl2 ToUserControl()
            {
                var userControl = new UserControl2();
                userControl.Title = Icerik;
                userControl.DosyaVarMi = DosyaVarMi;

            
            //Diğer özellikleri ayarlayabiliriz ek olarak istersek.
            return userControl;
            }
            public UserControl3 ToUserControl3()
            {
                var userControl = new UserControl3();
                userControl.Title = Icerik; 
                userControl.DosyaVarMi = DosyaVarMi; //Diğer özellikleri ayarlayabiliriz ek olarak istersek.
                                                     
            return userControl;
            }
    }

}

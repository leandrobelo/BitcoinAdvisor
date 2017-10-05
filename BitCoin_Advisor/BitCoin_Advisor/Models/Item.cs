using System;

namespace BitCoin_Advisor.Models
{
    public class Item : BaseDataObject
    {
        string text = string.Empty;
        public string Text
        {
            get { return text; }
            set { SetProperty(ref text, value); }
        }

        string valor = string.Empty;
        public string Valor
        {
            get { return valor; }
            set { SetProperty(ref this.valor, value); }
        }

        string data = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        public string Data
        {
            get { return data; }
            set { SetProperty(ref this.data, value); }
        }
    }
}

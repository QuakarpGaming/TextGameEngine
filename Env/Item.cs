using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGameEngine.Env
{
    public class Item
    {
        #region Constructors
        public Item()
        {
            Code = "Default";
            Description = "The dev should update this, please submit a bug report if you see this msg.";
            CodePrivate = "DEFAULT";
        }
        public Item(string code, string description)
        {
            Code = code.ToUpper();
            Description = description;
            CodePrivate = this.Code;
        }

        #endregion

        #region Data - Public
        public string Code { get { return this.CodePrivate; } set { this.CodePrivate = value.ToUpper(); } }
        private string CodePrivate { set; get; }
        public string Description { get; set; }
        #endregion


    }
}

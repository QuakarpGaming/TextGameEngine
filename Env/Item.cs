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
            PreventKill = new List<string>();
            KillMsg = string.Empty;
        }
        public Item(string code, string? description = null,bool canKill = false,List<string>? preventKill = null ,string? killMsg = null)
        {
            Code = code.ToUpper();
            Description = description ?? string.Empty;
            CodePrivate = this.Code;
            CanKill = canKill;
            PreventKill = preventKill ?? [];
            KillMsg = killMsg ?? "BLARG YOU ARE DEAD!";
        }

        #endregion

        #region Data - Public
        public string Code { get { return this.CodePrivate; } set { this.CodePrivate = value.ToUpper(); } }
        private string CodePrivate { set; get; }
        public string Description { get; set; }

        public bool CanKill { get; set; }
        public List<string> PreventKill { get; set; }
        public string KillMsg { get; set; }
        #endregion


    }
}

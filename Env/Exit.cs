using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TextGameEngine.Env
{
    public class Exit
    {
        #region Constructors
        public Exit()
        {
            this.ToRoomCode = string.Empty;
            this.KeyItemCode = string.Empty;
            this.IsLocked = false;
            this.ToRoomDiscription = string.Empty;
            this.LockMsg = string.Empty;
        }
        public Exit(string toRoomCode,string desc)
        {
            this.ToRoomCode = toRoomCode;
            this.KeyItemCode = string.Empty;
            this.IsLocked = false;
            this.ToRoomDiscription = desc;
            this.LockMsg = string.Empty;
        }
        public Exit(string toRoomCode, string toRoomDiscription, string keyItemCode, bool isLocked)
        {
            this.ToRoomCode = toRoomCode.ToUpper();
            this.KeyItemCode = keyItemCode.ToUpper();
            this.IsLocked = isLocked;
            this.ToRoomDiscription = toRoomDiscription;
            this.LockMsg = isLocked ? "Locked" : string.Empty;
        }
        public Exit(string toRoomCode, string toRoomDiscription, string keyItemCode, bool isLocked,string lockedMsg)
        {
            this.ToRoomCode = toRoomCode.ToUpper();
            this.KeyItemCode = keyItemCode.ToUpper();
            this.IsLocked = isLocked;
            this.ToRoomDiscription = toRoomDiscription;
            if (isLocked)
            {
                if(string.IsNullOrWhiteSpace(lockedMsg))
                {
                    this.LockMsg = "Locked";
                }
                else
                {
                    this.LockMsg = lockedMsg;
                }
            }
            else
            {
                this.LockMsg = string.Empty;
            }
        }

        #endregion

        #region Data
        public string ToRoomCode { get; set; }
        public string ToRoomDiscription {  get; set; }
        public string KeyItemCode { get; set; }
        public bool IsLocked { get; set; }
        public string LockMsg {  get; set; }

        #endregion

        #region Functions
        public void Unlock(List<Item> inv)
        {
            if (inv.Any(x => x.Code == this.KeyItemCode))
            {
                this.IsLocked = false;
            }
        }
        #endregion

        #region Printers
        public string Print()
        {
            //for debuging only
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Goint to Code: " + this.ToRoomCode);
            sb.AppendLine("Exit Desc: " + this.ToRoomDiscription);
            sb.AppendLine("Is locked: " + IsLocked.ToString());
            sb.AppendLine("Opened with: " + this.KeyItemCode);

            return sb.ToString();
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;


namespace CustomerCurrentUserField
{
    class CurrentUserField:SPFieldUser 
    {
        public CurrentUserField(SPFieldCollection fields, string typeName, string displayName)
            : base(fields, typeName, displayName)
        {

        }

        public CurrentUserField(SPFieldCollection fields, string fieldName) :
            base(fields, fieldName)
        {

        }
        

        public override string DefaultValue
        {
            get
            {
                SPWeb web = SPContext.Current.Web;
                SPUser user = web.CurrentUser;
                string defaultValue = string.Format("{0};#{1}", user.ID.ToString(), user.Name);
                if (this.SelectionGroup > 0)
                {
                    SPGroup group = web.Groups[this.SelectionGroup];
                    if ((group != null) && (group.ContainsCurrentUser))
                    {
                        return defaultValue;
                    }
                }
                else
                {
                    return defaultValue;
                }
                return string.Empty;
            }
            set
            {
                base.DefaultValue = value;
            }
        }

    }
}

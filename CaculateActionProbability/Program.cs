using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaculateActionProbability
{
    class Program
    {
        static void Main(string[] args)
        {
            CaculateActionProbability();
        }
        /// <summary>
        /// 计算操作概率,UserID,ActionID,TotalNums
        /// </summary>
        private static void CaculateActionProbability()
        {
            //VSDLL.DAL.DataProvider.ConnectionString = ConfigurationManager.ConnectionStrings["SqlProviderVS"].ConnectionString;
            DataSet ds = VSDLL.DAL.ActionDAL.GetActionProbability();
            DataSet dsActions = VSDLL.DAL.ActionDAL.GetAllActions();
            DataSet dsUserActions = VSDLL.DAL.ActionDAL.GetUserActionByUserID(0);
            Dictionary<int, int> actionCounts = new Dictionary<int, int>();
            DataSet dsActionChanged = dsActions.Clone();
            DataSet dsUserActionsChanged = dsUserActions.Clone();
            int actionID;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                actionID = (int)dr["ActionID"];
                DataRow[] drs = dsUserActions.Tables[0].Select("ActionID=" + actionID + " and UserID=" + dr["UserID"]);
                if (drs.Length > 0)
                {
                    drs[0]["Probability"] = dr["TotalNums"];
                    drs[0]["Modified"] = DateTime.Now;
                    //drs[0]["ModifiedBy"] = VSDLL.Common.Users.UserID;
                    dsUserActionsChanged.Merge(drs);
                }
                if (actionCounts.ContainsKey(actionID))
                    actionCounts[actionID] = actionCounts[actionID] + (int)dr["TotalNums"];
                else
                    actionCounts.Add(actionID, (int)dr["TotalNums"]);
            }
            DataRow[] drsActions;
            foreach (int keyID in actionCounts.Keys)
            {
                drsActions = dsActions.Tables[0].Select("ActionID=" + keyID);
                if (drsActions.Length > 0)
                {
                    drsActions[0]["Probability"] = actionCounts[keyID];
                    drsActions[0]["Modified"] = DateTime.Now;
                    //drsActions[0]["ModifiedBy"] = VSDLL.Common.Users.UserID;
                    dsActionChanged.Merge(drsActions);
                }
            }
            VSDLL.BLL.ActionBLL.UdateActionProbability(dsActionChanged.Tables[0], dsUserActionsChanged.Tables[0]);
            Console.WriteLine("ok");
        }
    }
}

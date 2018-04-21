using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SN_Manager
{
    class ExportOrderToCsv
    {
        public ExportOrderToCsv()
        {

        }

        public void ExportOrderByOrderName(string orderName, string saveRoad)
        {
            DataTable orderInfo = Program.sql.ExecuteQuery("select MachineModel from orderTable where OrderName = '" + orderName + "'");
            string machineModel = orderInfo.Rows[0]["MachineModel"].ToString();

            TextWriter tw1 = new StreamWriter(saveRoad + "/" + orderName + ".csv", false);//如果该文件不存在，则新建文件。如果存在，以覆盖形式写入文件。
            tw1.WriteLine("Robot SN,Charge Base SN,MODEL,WORKORDER,DATETIME,");

            string newDirRoad = saveRoad + "/" + orderName + "文件夹";
            if (!Directory.Exists(newDirRoad))//如果不存在该文件夹，则创建一个新的
            {
                Directory.CreateDirectory(newDirRoad);
            }
            DataTable orderContent = Program.sql.ExecuteQuery("select * from '" + orderName + "'");
            for(int i = 0; i < orderContent.Rows.Count; i++)
            {
                string robotSn = orderContent.Rows[i]["RobotSN"].ToString();
                string chargeBaseSN = orderContent.Rows[i]["chargeBaseSN"].ToString();
                string time = orderContent.Rows[i]["DATETIME"].ToString();
                string newFileRoad = newDirRoad + "/" + robotSn + ".csv";
                TextWriter tw2 = new StreamWriter(newFileRoad,false);//如果该文件不存在，则新建文件。如果存在，以覆盖形式写入文件。
                tw2.WriteLine("Robot SN,Charge Base SN,MODEL,DATETIME,");
                tw2.WriteLine(robotSn+","+chargeBaseSN+","+ machineModel +"," + time+",");
                tw2.Flush();
                tw2.Close();

                tw1.WriteLine(robotSn + "," + chargeBaseSN + "," + machineModel + ","+ orderName + ","+ time + ",");
            }
            tw1.Flush();
            tw1.Close();
            return;
        }

        public void ExportAllOrder()
        {

        }
    }
}

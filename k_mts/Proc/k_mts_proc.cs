using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Data;
using System.Globalization;

namespace k_mts
{
    using Field_Step4 = k_mts_proc.Field_Step3;
    using Field_Step3_2 = k_mts_proc.Field_Step3_1;
    using Field_Step5_1 = k_mts_proc.Field_Step4_1;

    public class k_mts_proc : Dictionary<string, DataTable>
    {
        #region Property

        public int Term { get; protected set; }

        #endregion

        #region Constructor

        private k_mts_proc()
        {
            // It should be initialized
        }

        public k_mts_proc(int term)
        {
            Term = term;
        }

        #endregion

        #region Calculate Procedure

        // input : Euro_5
        // output : Euro_5_1 ... Euro_5_6
        public void Calculate_Historical(string tblName)
        {
            try
            {
                var dt0 = Global.DB.Tables[tblName];
                if (dt0 == null) return;

                var dt = dt0;

                var step1 = ProcStep1(dt);
                var step2 = ProcStep2(step1);
                var step3 = ProcStep3(step2);
                var step4 = ProcStep4(step3);
                var step5 = ProcStep5(step4, Term);

                MakeSmoothLine(step5);
                dt = step5.ToTable<k_mts_proc.Field_Step5>(0.0, "idx");
                dt.TableName = dt0.TableName + "_1";
                dt.Namespace = dt0.TableName;
                Global.DB.Tables.Add(dt);

                var step3_1 = ProcStep3_1(step3);
                var step4_1 = ProcStep4_1(step3_1, 2);
                var step5_1 = ProcStep5_1(step4_1);
                var step6_1 = ProcStep6_1(step5_1);

                MakeSmoothLine(step6_1);
                dt = step6_1.ToTable<k_mts_proc.Field_Step6_1>(0.0, "idx");
                dt.TableName = dt0.TableName + "_2";
                dt.Namespace = dt0.TableName;
                Global.DB.Tables.Add(dt);

                for (int i = 0; i < 4; i++)
                {
                    var step3_2 = ProcStep3_2(step4_1);
                    step4_1 = ProcStep4_1(step3_2, i + 3);
                    step5_1 = ProcStep5_1(step4_1);
                    step6_1 = ProcStep6_1(step5_1);

                    MakeSmoothLine(step6_1);
                    dt = step6_1.ToTable<k_mts_proc.Field_Step6_1>(0.0, "idx");
                    dt.TableName = dt0.TableName + "_" + (i + 3);
                    dt.Namespace = dt0.TableName;
                    Global.DB.Tables.Add(dt);
                }
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.ToString(), "k_mts_Proc::" + Term);
            }
        }

        #endregion

        #region Step별 Field 정의

        public struct Field_Step1
        {
            public string hr;
            public string targetDate;
            public string targetTime;
            public double start;
            public double high;
            public double low;
            public double stop;

            public string c1;
            public string c2;
            public double c3;
            public int c4;
        }
        public struct Field_Step2
        {
            public string c1;
            public int c2;
            public string c3;
            public double c4;
            public double c5;
        }
        public struct Field_Step3
        {
            public int c0;
            public string c1;
            public int c2;
            public string c3;
            public double c4;

            public string c5;
            public string c6;
            public string c7;
            public double c8;
            public double c9;
            public double c10;
        }
        public struct Field_Step5
        {
            public string color;
            public DateTime date;
            public double value;
            public double ss;
            public double e;
            public double x;
            public double dx;
            public double ss_line;
        }
        public struct Field_Step3_1
        {
            public int c0;
            public int c1;
            public string c2;
            public double c3;
            public string 외;
        }
        public struct Field_Step4_1
        {
            public int c0;
            public string c1;
            public double c2;
            public string c3;
            public string c4;
            public string c5;
            public string 외;
        }
        public struct Field_Step6_1
        {
            public string color;
            public DateTime date;
            public double value;
            public double ss;
            public double ss_line;
        }

        #endregion

        #region MakeSmoothLine (ss_line)

        public void MakeSmoothLine(Field_Step5[] arr, int r_start = 0, double NullValue = 0.0)
        {
            int start = -1;
            int nRow = arr.Length;
            if (nRow == 0) return;

            for (int r = r_start; r < nRow; r++)
            {
                double val = arr[r].ss;
                if (val == NullValue) continue;
                else
                {
                    arr[r].ss_line = val;

                    if (!(start < 0 || r - start == 1))
                    {
                        double y_start = arr[start].ss;
                        double dy = (val - y_start) / (r - start);

                        for (int i = start + 1; i < r; i++)
                            arr[i].ss_line = y_start + dy * (i - start);
                    }
                    start = r;
                }
            }
        }

        public void MakeSmoothLine(Field_Step6_1[] arr, int r_start = 0, double NullValue = 0.0)
        {
            int start = -1;
            int nRow = arr.Length;
            if (nRow == 0) return;

            for (int r = r_start; r < nRow; r++)
            {
                double val = arr[r].ss;
                if (val == NullValue) continue;
                else
                {
                    arr[r].ss_line = val;

                    if (!(start < 0 || r - start == 1))
                    {
                        double y_start = arr[start].ss;
                        double dy = (val - y_start) / (r - start);

                        for (int i = start + 1; i < r; i++)
                            arr[i].ss_line = y_start + dy * (i - start);
                    }
                    start = r;
                }
            }
        }

        #endregion

        #region Step별 Proc 정의

        public Field_Step1[] ProcStep1(DataTable dt)
        {
            try
            {
                var rows = dt.Rows;
                int nRow = rows.Count;

                Field_Step1[] arr = new Field_Step1[nRow];

                // 원본 테이블로 부터 Step1배열을 초기화 시킵니다.				
                for (int r = 0; r < nRow; r++)
                {
                    DataRow item = rows[r];
                    arr[r].hr = item[0].ToString();
                    arr[r].targetDate = Convert.ToDateTime(item[1]).ToString("yyyy-MM-dd");
                    arr[r].targetTime = Convert.ToDateTime(item[2]).ToString("HH:mm");
                    arr[r].start = Convert.ToDouble(item[3]);
                    arr[r].high = Convert.ToDouble(item[4]);
                    arr[r].low = Convert.ToDouble(item[5]);
                    arr[r].stop = Convert.ToDouble(item[6]);
                }

                //------------------------------------------------------------
                // Field_Step1 : c1 ~ c4
                //------------------------------------------------------------
                for (int r = 0; r < nRow; r++)
                {
                    double si = arr[r].start;
                    double jong = arr[r].stop;

                    #region c1

                    //arr[r].c1 = jong >= si ? "H" : "R";

                    #endregion

                    #region C2

                    if (r > 0)
                    {
                        string res = "";

                        if (arr[r].high > arr[r - 1].high && arr[r].low >= arr[r - 1].low)
                            res = "H";
                        else if (arr[r].high <= arr[r - 1].high && arr[r].low < arr[r - 1].low)
                            res = "R";
                        else if (arr[r].high <= arr[r - 1].high && arr[r].low >= arr[r - 1].low)
                            res = "N";
                        else if (arr[r].high > arr[r - 1].high && arr[r].low < arr[r - 1].low && arr[r].hr.Equals("a"))
                            res = "HH";
                        else if (arr[r].high > arr[r - 1].high && arr[r].low < arr[r - 1].low && arr[r].hr.Equals("aaa")) // 상,하 중 하나
                            res = "HH";
                        else if (arr[r].high > arr[r - 1].high && arr[r].low < arr[r - 1].low && arr[r].hr.Equals("aaa2")) // 상,하 모두
                            res = "HH";
                        else if (arr[r].high > arr[r - 1].high && arr[r].low < arr[r - 1].low && arr[r].hr.Equals("bba"))
                            res = "HH";
                        else if (arr[r].high > arr[r - 1].high && arr[r].low < arr[r - 1].low && arr[r].hr.Equals("b"))
                            res = "RR";
                        else if (arr[r].high > arr[r - 1].high && arr[r].low < arr[r - 1].low && arr[r].hr.Equals("aab")) // 상,하 중 하나
                            res = "RR";
                        else if (arr[r].high > arr[r - 1].high && arr[r].low < arr[r - 1].low && arr[r].hr.Equals("aab2")) // 상,하 모두
                            res = "RR";
                        else if (arr[r].high > arr[r - 1].high && arr[r].low < arr[r - 1].low && arr[r].hr.Equals("bbb"))
                            res = "RR";
                        else
                            res = "0";

                        arr[r].c2 = res;
                    }
                    else
                        arr[r].c2 = "";

                    #endregion

                    #region c3

                    //=IF(AND(I2="H"),E2,IF(AND(I2="R"),F2,
                    //IF(AND(I1="H",I2="N"),F2,
                    //IF(AND(I1="R",I2="N"),E2,
                    //IF(AND(I2="RR"),E2,
                    //IF(AND(I2="HH"),F2,
                    //IF(AND(I1="RR",I2="N"),E2,
                    //IF(AND(I1="HH",I2="N"),F2,
                    //IF(AND(I1="N",I2="N",J1=E1),F2,
                    //IF(AND(I1="N",I2=

                    //=IF(AND(I2="H"),E2,IF(AND(I2="R"),F2,IF(AND(I1="H",I2="N"),F2,IF(AND(I1="R",I2="N"),E2,IF(AND(I2="RR"),E2,IF(AND(I2="HH"),F2,IF(AND(I1="RR",I2="N"),E2,IF(AND(I1="HH",I2="N"),F2,IF(AND(I1="N",I2="N",J1=E1),F2,
                    //IF(AND(I1="N",I2="N",J1=F1),E2,0))))))))))

                    if (r > 0)
                    {
                        double c3 = 0.0;

                        if (arr[r].c2 == "H")
                            c3 = arr[r].high;
                        else if (arr[r].c2 == "R")
                            c3 = arr[r].low;
                        else if (arr[r - 1].c2 == "H" && arr[r].c2 == "N")
                            c3 = arr[r].low;
                        else if (arr[r - 1].c2 == "R" && arr[r].c2 == "N")
                            c3 = arr[r].high;
                        else if (arr[r].c2 == "RR")
                            c3 = arr[r].high;
                        else if (arr[r].c2 == "HH")
                            c3 = arr[r].low;
                        else if (arr[r - 1].c2 == "RR" && arr[r].c2 == "N")
                            c3 = arr[r].high;
                        else if (arr[r - 1].c2 == "HH" && arr[r].c2 == "N")
                            c3 = arr[r].low;
                        else if (arr[r - 1].c2 == "N" && arr[r].c2 == "N" && arr[r - 1].c3 == arr[r - 1].high)
                            c3 = arr[r].low;
                        else if (arr[r - 1].c2 == "N" && arr[r].c2 == "N" && arr[r - 1].c3 == arr[r - 1].low)
                            c3 = arr[r].high;
                        else
                            c3 = 0.0;

                        arr[r].c3 = c3;
                    }

                    #endregion

                    #region c4

                    if (arr[r].c3 == arr[r].high)
                        arr[r].c4 = 2;
                    else if (arr[r].c3 == arr[r].low)
                        arr[r].c4 = 1;

                    #endregion
                }
                return arr;
            }
            catch (Exception ex) { throw new Exception("step1 데이터가 없습니다.", ex); }
        }

        public Field_Step2[] ProcStep2(Field_Step1[] step1)
        {
            try
            {
                int nRow = step1.Length;
                int c2Sum = 0;

                Field_Step2[] arr = new Field_Step2[nRow];

                for (int r = 0; r < nRow; r++)
                {
                    #region c1
                    //======================  여기부터 시작 됨..  QQQ
                    string c1 = string.Empty;
                    //if (step1[r].c2 != "HH" && step1[r].c2 != "RR" && step1[r].hr != "aaa" && step1[r].hr != "aaa2" && step1[r].hr != "aab" &&
                    //   step1[r].hr != "aab2" && step1[r].hr != "bba" && step1[r].hr != "bbb" && step1[r].hr != "aa1" && step1[r].hr != "aa2" &&
                    //   step1[r].hr != "bb1" && step1[r].hr != "bb2" && step1[r].hr != "cc1" && step1[r].hr != "cc2" && step1[r].hr != "dd1" &&
                    //    step1[r].hr != "dd2" && step1[r].hr != "ee1" && step1[r].hr != "ee2" && step1[r].hr != "ff1" && step1[r].hr != "ff2")
                    //    c1 = "0";
                  
                     if (step1[r].hr == "aaa")
                        c1 = "aaa";
                    else if (step1[r].hr == "aaa2")
                        c1 = "aaa2";
                    else if (step1[r].hr == "aab")
                        c1 = "aab";
                    else if (step1[r].hr == "aab2")
                        c1 = "aab2";
                    else if (step1[r].hr == "bba")
                        c1 = "bba";
                    else if (step1[r].hr == "bbb")
                        c1 = "bbb";
                    else if (step1[r].hr == "aa1")
                        c1 = "aa1";
                    else if (step1[r].hr == "aa2")
                        c1 = "aa2";
                    else if (step1[r].hr == "bb1")
                        c1 = "bb1";
                    else if (step1[r].hr == "bb2")
                        c1 = "bb2";
                    else if (step1[r].hr == "cc1")
                        c1 = "cc1";
                    else if (step1[r].hr == "cc2")
                        c1 = "cc2";
                    else if (step1[r].hr == "dd1")
                        c1 = "dd1";
                    else if (step1[r].hr == "dd2")
                        c1 = "dd2";
                    else if (step1[r].hr == "ee1")
                        c1 = "ee1";
                    else if (step1[r].hr == "ee2")
                        c1 = "ee2";
                    else if (step1[r].hr == "ff1")
                        c1 = "ff1";
                    else if (step1[r].hr == "ff2")
                        c1 = "ff2";
                    else if(step1[r].c2 == "HH" || step1[r].c2 == "RR")
                        c1 = "HR";
                    else
                        c1 = "0";
                    //else if (item[r].hr.ToString() == "H7" || item[r].hr.ToString() == "R7")
                    //    c1 = "7";
                    //else if (item[r].hr.ToString() == "3")
                    //    c1 = "3";
                    //else if (item[r].hr.ToString() == "6")
                    //    c1 = "6";
                    //else
                    //    c1 = "0";

                    #endregion

                    #region c3

                    string c3 = string.Empty;
                    c3 = string.Format("{0}_{1}", step1[r].targetDate, step1[r].targetTime);

                    #endregion

                    #region c4

                    //IF(AND(K5337=K5338,I5337="HH"),J5337,IF(AND(K5337=K5338,I5337="RR"),J5337,IF(AND(K5337=K5338),0,J5337)))
                    double c4 = 0.0;
                    if (r > 0 && r < nRow)
                    {
                        int nValue = 0;
                        if (r + 1 < nRow)
                            nValue = (int)step1[r + 1].c4;

                        if ((int)step1[r].c4 == nValue && step1[r].c2 == "HH")
                            c4 = step1[r].c3;
                        else if ((int)step1[r].c4 == nValue && step1[r].c2 == "RR")
                            c4 = step1[r].c3;
                        else if ((int)step1[r].c4 == nValue)
                            c4 = 0.0;
                        else
                            c4 = step1[r].c3;
                    }

                    #endregion

                    #region c5

                    var c5 = 0.0;
                    if (r > 0 && r < nRow - 1)
                    {
                        if (step1[r].c2 == "HH" && step1[r + 1].c2 == "R")
                            c5 = step1[r].high;
                        else if (step1[r].c2 == "HH" && step1[r + 1].c2 == "N")
                            c5 = step1[r].high;
                        else if (step1[r].c2 == "HH" && step1[r + 1].c2 == "HH")
                            c5 = step1[r].high;
                        else if (step1[r].c2 == "RR" && step1[r + 1].c2 == "H")
                            c5 = step1[r].low;
                        else if (step1[r].c2 == "RR" && step1[r + 1].c2 == "N")
                            c5 = step1[r].low;
                        else if (step1[r].c2 == "RR" && step1[r + 1].c2 == "RR")
                            c5 = step1[r].low;
                        else
                            c5 = 0.0;
                    }
                    // 여기까지 오늘 8-13
                    #endregion

                    #region c2

                    var c2 = 0;
                    if (r > 0)
                    {

                        if (c4 > 0)   // (c4 > 1) 을 수정
                            c2 = r - c2Sum;
                        else
                            c2 = 0;
                    }

                    c2Sum += c2;

                    #endregion

                    arr[r].c1 = c1;
                    arr[r].c2 = c2;
                    arr[r].c3 = c3;
                    arr[r].c4 = c4;
                    arr[r].c5 = c5;
                }
                return arr;
            }
            catch (Exception ex) { throw new Exception("step2 데이터가 없습니다.", ex); }
        }

        public Field_Step3[] ProcStep3(Field_Step2[] step2)
        {
            try
            {
                List<Field_Step3> lst = new List<Field_Step3>();
                //------------------------------------------------------------
                // Field_Step3 : c1 ~ c4
                //------------------------------------------------------------
                int nRow = step2.Length;
                for (int r = 0; r < nRow; r++)
                {
                    if (step2[r].c4 <= 0.0)
                        continue;

                    #region c1, c2, c3, c4, c5
                    {
                        Field_Step3 row = new Field_Step3();
                        row.c1 = step2[r].c1;
                        row.c2 = step2[r].c2;
                        row.c3 = step2[r].c3.Split(' ')[0]; // date
                        row.c4 = step2[r].c4;
                        lst.Add(row);
                    }
                    #endregion

                    if (step2[r].c5 > 0.0)
                    {
                        Field_Step3 row = new Field_Step3();
                        row.c1 = step2[r].c1;
                        row.c2 = step2[r].c2;
                        row.c3 = step2[r].c3.Split(' ')[0]; // date
                        row.c4 = step2[r].c5; // !!!!!
                        lst.Add(row);
                    }
                }
                Field_Step3[] item = lst.ToArray();

                #region c5, c6

                //------------------------------------------------------------
                // Field_Step3 : c0, c5, c6
                //-----------------------------------------------------------
                nRow = item.Length;
                for (int r = 0; r < nRow; r++)
                {
                    #region c0

                    var c0 = item[r].c2;
                    //IF(AND(AF8="HR",AF9="HR",AI8=AI9),1,AG9)

                    //====================== 이 부분참고..  148줄 아래 참고 QQQ
                    if (r > 0)
                    {
                        if
                         (item[r].c1 == "0")

                            item[r].c0 = c0;
                        else if
                            (item[r].c1 == "HR" &&
                            item[r - 1].c1 == "HR" &&
                            item[r].c3 == item[r - 1].c3)
                            c0 = 1;
                        else if (item[r].c1 == "aaa" &&
                            item[r - 1].c1 == "aaa" &&
                            item[r].c3 == item[r - 1].c3)
                            c0 = 1;
                        else if (item[r].c1 == "aaa2" &&
                            item[r - 1].c1 == "aaa2" &&
                            item[r].c3 == item[r - 1].c3)
                            c0 = 1;
                        else if (item[r].c1 == "aab" &&
                            item[r - 1].c1 == "aab" &&
                            item[r].c3 == item[r - 1].c3)
                            c0 = 1;
                        else if (item[r].c1 == "aab2" &&
                           item[r - 1].c1 == "aab2" &&
                           item[r].c3 == item[r - 1].c3)
                            c0 = 1;
                        else if (item[r].c1 == "bba" &&
                            item[r - 1].c1 == "bba" &&
                            item[r].c3 == item[r - 1].c3)
                            c0 = 1;
                        else if (item[r].c1 == "bbb" &&
                            item[r - 1].c1 == "bbb" &&
                            item[r].c3 == item[r - 1].c3)
                            c0 = 1;
                    }
                    item[r].c0 = c0;

                    #endregion

                    #region c5 process

                    //IF(
                    //  AND(
                    //    OR(AJ10<AJ8,
                    //      AND(AJ10>=AJ8,AJ11<=AJ9,AJ12<AJ10)
                    //      ,AND(AJ9>=AJ11,AJ9>=AJ13,AJ12>=AJ10,AJ14<AJ12)
                    //      ,AND(AJ9>=AJ11,AJ9>=AJ13,AJ9>=AJ15,AJ14>=AJ12,AJ16<AJ14)
                    //      ,AND(AJ9>=AJ11,AJ9>=AJ13,AJ9>=AJ15,AJ9>=AJ17,AJ16>=AJ14,AJ18<AJ16)
                    //      ,AND(AJ9>=AJ11,AJ9>=AJ13,AJ9>=AJ15,AJ9>=AJ17,AJ9>=AJ19,AJ18>=AJ16,AJ20<AJ18)
                    //      ,AND(AJ9>=AJ11,AJ9>=AJ13,AJ9>=AJ15,AJ9>=AJ17,AJ9>=AJ19,AJ9>=AJ21,AJ20>=AJ18,AJ22<AJ20)
                    //  ),AJ9>AJ8,AJ9>AJ7),"H",0)

                    double[] arr = new double[23];  //**수정사항 //21
                    for (int j = 0; j < 23; j++)
                    {
                        int i = r - 5 + j;
                        if (i < 0 || i > nRow - 1)
                            arr[j] = 0;
                        else
                            arr[j] = item[i].c4;
                    }
                    item[r].c5 = GetC5(arr, item[r].c1); // idx_chart = 1 (default)

                    #endregion

                    #region c6

                    double[] arr2 = new double[19];
                    for (int j = 0; j < 19; j++)
                    {
                        int i = r - 15 + j;
                        if (i < 0 || i > nRow - 1)
                            arr2[j] = 0;
                        else
                            arr2[j] = item[i].c4;
                    }
                    //IF(
                    //  AND(
                    //      OR(
                    //          arr2[16]<arr2[14],
                    //          AND(arr2[15]>=arr2[17],arr2[16]>arr2[18],arr2[16]>=arr2[14]),
                    //          AND(arr2[16]>=arr2[14],arr2[17]<=arr2[15],arr2[18]>=arr2[16])
                    //      ),
                    //      arr2[15]>arr2[14],
                    //      arr2[15]>arr2[13],
                    //      arr2[13]>arr2[11],
                    //      arr2[11]>arr2[9],
                    //      arr2[9]>arr2[7],
                    //      arr2[7]>arr2[5],
                    //      arr2[5]>arr2[3],
                    //      arr2[3]>arr2[1],
                    //      arr2[14]>=arr2[12],
                    //      arr2[12]>=arr2[10],
                    //      arr2[10]>=arr2[8],
                    //      arr2[8]>=arr2[6],
                    //      arr2[6]>=arr2[4],
                    //      arr2[4]>=arr2[2],
                    //      arr2[2]>=arr2[0]
                    //  ),
                    //"고15",
                    //IF(
                    //  AND(
                    //      OR(arr2[16]>arr2[14],
                    //        AND(arr2[15]<=arr2[17],arr2[16]<arr2[18],arr2[16]<=arr2[14]),
                    //        AND(arr2[16]<=arr2[14],arr2[17]>=arr2[15],arr2[18]<=arr2[16])
                    //      ),
                    //      arr2[15]<arr2[14],
                    //      arr2[15]<arr2[13],
                    //      arr2[13]<arr2[11],
                    //      arr2[14]<=arr2[12],
                    //      arr2[12]<=arr2[10]
                    //  ),
                    //"R5",0))

                    item[r].c6 = GetC6(arr2);

                    #endregion
                }

                #endregion

                #region c7, c8, c9, c10

                //------------------------------------------------------------
                // Field_Step3 : c7 ~ c10
                //-----------------------------------------------------------
                for (int r = 0; r < nRow; r++)
                {
                    #region c7 process

                    string[] arr = new string[10];
                    for (int j = 0; j < 10; j++)
                    {
                        int i = r + j;
                        if (i < 0 || i > nRow - 1)
                            arr[j] = "0";
                        else
                            arr[j] = item[i].c6;
                    }
                    item[r].c7 = GetC7(arr);

                    #endregion

                    #region c8

                    double c8 = 0.0;
                    if (r > 0)
                    {
                        if (
                            // (item[1].ToString() == "HR") ||  (pitem[1].ToString() == "HR" && 
                            (item[r].c1 == "HR" || item[r].c1 == "aaa" || item[r].c1 == "aaa2" || item[r].c1 == "aab" || item[r].c1 == "aab2" ||
                            item[r].c1 == "bba" || item[r].c1 == "bbb") && item[r - 1].c3 != item[r].c3)
                            c8 = item[r].c4;// (item[4] * 0.0002);//수정
                        else
                            c8 = 0.0; // na
                    }
                    item[r].c8 = c8;

                    #endregion

                    #region c9
                    //IF(OR(AND(AF15="HR",AF16="HR",AI15=AI16),AND(AF15=7,AF16="HR",AI15=AI16),AND(AF15="HR",AF16=7,AI15=AI16)),AJ16-0.3,NA())
                    double c9 = 0.0;
                    if (r > 0)
                    {
                        if (item[r].c3 == item[r - 1].c3 && item[r].c1 == "HR" && item[r - 1].c1 == "HR")
                            c9 = item[r].c4;// - (item[4] * 0.0002);//수정
                        else if (item[r].c3 == item[r - 1].c3 && item[r].c1 == "aaa" && item[r - 1].c1 == "aaa")
                            c9 = item[r].c4;
                        else if (item[r].c3 == item[r - 1].c3 && item[r].c1 == "aaa2" && item[r - 1].c1 == "aaa2")
                            c9 = item[r].c4;
                        else if (item[r].c3 == item[r - 1].c3 && item[r].c1 == "aab" && item[r - 1].c1 == "aab")
                            c9 = item[r].c4;
                        else if (item[r].c3 == item[r - 1].c3 && item[r].c1 == "aab2" && item[r - 1].c1 == "aab2")
                            c9 = item[r].c4;
                        else if (item[r].c3 == item[r - 1].c3 && item[r].c1 == "bba" && item[r - 1].c1 == "bba")
                            c9 = item[r].c4;
                        else if (item[r].c3 == item[r - 1].c3 && item[r].c1 == "bbb" && item[r - 1].c1 == "bbb")
                            c9 = item[r].c4;
                        //else if (item[r].c3 == item[r - 1].c3 && item[r].c1 == "bb1" && item[r - 1].c1 == "bb1")
                        //    c9 = item[r].c4;
                        else
                            c9 = 0.0; // na()
                    }
                    item[r].c9 = c9;
                    #endregion

                    #region c10

                    double c10 = 0.0;
                    if (r < nRow - 1)
                    {
                        // if (item[3].ToString() != nItem[3].ToString())
                        if (item[r].c3.Substring(9, 1) != item[r + 1].c3.Substring(9, 1)) // 8-12 수정 9
                            c10 = item[r].c4; // (item[4] * 0.0005);//수정
                        else
                            c10 = 0.0; // na()
                    }
                    item[r].c10 = c10;

                    #endregion
                }

                #endregion

                return item;
            }
            catch (Exception ex) { throw new Exception("step3 데이터가 없습니다.", ex); }
        }

        private string GetC5(double[] arr, string item1 = "", int idx_chart = 1)
        {
            string c5 = null;

            #region c5 process

            switch (idx_chart) // idx_chart : 차트번호에 따라 if문을 달리함.
            {
                case 1:
                    if (item1 == "aaa")
                        c5 = "aaa";
                    else if (item1 == "aaa2")
                        c5 = "aaa2";
                    break;

                case 2:
                    if (item1 == "bba")
                        c5 = "bba";
                    else if (item1 == "bb2")
                        c5 = "bb2";
                    break;

                case 3:
                    if (item1 == "ccc")
                        c5 = "ccc";
                    else if (item1 == "cc1")
                        c5 = "cc1";
                    else if (item1 == "cc2")
                        c5 = "cc2";
                    break;

                case 4:
                    if (item1 == "ddd")
                        c5 = "ddd";
                    else if (item1 == "dd1")
                        c5 = "dd1";
                    else if (item1 == "dd2")
                        c5 = "dd2";
                    break;

                case 5:
                    if (item1 == "eee")
                        c5 = "eee";
                    else if (item1 == "ee1")
                        c5 = "ee1";
                    else if (item1 == "ee2")
                        c5 = "ee2";
                    break;

                case 6:
                    if (item1 == "fff")
                        c5 = "fff";
                    else if (item1 == "ff1")
                        c5 = "ff1";
                    else if (item1 == "ff2")
                        c5 = "ff2";
                    break;
            }

            if (c5 != null) return c5; // 위에서 c5값이 결정된 경우 바로 반환
            else if (arr[5] > arr[4])
            {


                if (
                        (arr[5] > arr[4] && arr[5] > arr[3]) &&
                        (arr[6] < arr[4] ||
                        (arr[6] >= arr[4] && arr[7] <= arr[5] && arr[8] < arr[6]) ||
                        (arr[5] >= arr[7] && arr[5] >= arr[9] && arr[8] >= arr[6] && arr[10] < arr[8]) ||
                        (arr[5] >= arr[7] && arr[5] >= arr[9] && arr[5] >= arr[11] && arr[10] >= arr[8] && arr[12] < arr[10]) ||
                        (arr[5] >= arr[7] && arr[5] >= arr[9] && arr[5] >= arr[11] && arr[5] >= arr[13] && arr[12] >= arr[10] && arr[14] < arr[12]) ||
                        (arr[5] >= arr[7] && arr[5] >= arr[9] && arr[5] >= arr[11] && arr[5] >= arr[13] && arr[5] >= arr[15] && arr[14] >= arr[12] && arr[16] < arr[14]) ||
                        (arr[5] >= arr[7] && arr[5] >= arr[9] && arr[5] >= arr[11] && arr[5] >= arr[13] && arr[5] >= arr[15] && arr[5] >= arr[17] && arr[16] >= arr[14] && arr[18] < arr[16])))

                    c5 = "H";    //M고


                else
                    c5 = "0";
            }
            #endregion

            return c5;
        }

        private string GetC6(double[] arr2)
        {
            var c6 = "0";

            if (arr2[15] > arr2[14])
            {


               if (
                     (arr2[15] > arr2[14] && arr2[15] > arr2[13] && arr2[14] >= arr2[12]) &&
                                (arr2[16] < arr2[14] ||
                                (arr2[16] >= arr2[14] && arr2[17] < 0.1) ||
                                (arr2[16] >= arr2[14] && arr2[17] <= arr2[15])))

                    c6 = "H3";   
            

                else if (
               (arr2[14] > arr2[12] && arr2[15] >= arr2[13] && arr2[16] <= arr2[14] && arr2[16] > arr2[15] ||//상미A"
               (arr2[12] > arr2[10] && arr2[13] >= arr2[11] && arr2[14] <= arr2[12] && arr2[15] >= arr2[13] && arr2[16] <= arr2[14] && arr2[16] > arr2[15]) ||
               (arr2[10] > arr2[8] & arr2[11] >= arr2[9] && arr2[12] <= arr2[10] && arr2[13] >= arr2[11] && arr2[14] <= arr2[12] && arr2[15] >= arr2[13] &&
                   arr2[16] <= arr2[14] && arr2[16] > arr2[15]) ||
               (arr2[8] > arr2[6] && arr2[9] >= arr2[7] && arr2[10] <= arr2[8] && arr2[11] >= arr2[9] && arr2[12] <= arr2[10] && arr2[13] >= arr2[11] &&
                   arr2[14] <= arr2[12] && arr2[15] >= arr2[13] && arr2[16] <= arr2[14] && arr2[16] > arr2[15]) ||
               (arr2[13] > arr2[11] && arr2[14] >= arr2[12] & arr2[15] <= arr2[13] && arr2[15] > arr2[14]) || //상미B";
               (arr2[11] > arr2[9] && arr2[12] >= arr2[10] && arr2[13] <= arr2[11] && arr2[14] >= arr2[12] && arr2[15] <= arr2[13] && arr2[15] > arr2[14]) ||
               (arr2[9] > arr2[7] && arr2[10] >= arr2[8] && arr2[11] <= arr2[9] && arr2[12] >= arr2[10] && arr2[13] <= arr2[11] && arr2[14] >= arr2[12] &&
                   arr2[15] <= arr2[13] && arr2[15] > arr2[14]) ||
               (arr2[7] > arr2[5] && arr2[8] >= arr2[6] && arr2[9] <= arr2[7] && arr2[10] >= arr2[8] && arr2[11] <= arr2[9] && arr2[12] >= arr2[10] &&
                   arr2[13] <= arr2[11] && arr2[14] >= arr2[12] && arr2[15] <= arr2[13] && arr2[15] > arr2[14])))

                    c6 = "SM";  

                else if (
                       (arr2[14] < arr2[12] && arr2[15] <= arr2[13] && arr2[16] >= arr2[14] && arr2[16] < arr2[15] ||   //하미A";
                       (arr2[12] < arr2[10] && arr2[13] <= arr2[11] && arr2[14] >= arr2[12] && arr2[15] <= arr2[13] && arr2[16] >= arr2[14] && arr2[16] < arr2[15]) ||
                       (arr2[10] < arr2[8] && arr2[11] <= arr2[9] && arr2[12] >= arr2[10] && arr2[13] <= arr2[11] && arr2[14] >= arr2[12] && arr2[15] <= arr2[13] &&
                           arr2[16] >= arr2[14] && arr2[16] < arr2[15]) ||
                       (arr2[8] < arr2[6] && arr2[9] <= arr2[7] && arr2[10] >= arr2[8] && arr2[11] <= arr2[9] && arr2[12] >= arr2[10] && arr2[13] <= arr2[11] &&
                           arr2[14] >= arr2[12] && arr2[15] <= arr2[13] && arr2[16] >= arr2[14] && arr2[16] < arr2[15]) ||
                       (arr2[13] < arr2[11] && arr2[14] <= arr2[12] && arr2[15] >= arr2[13] && arr2[15] < arr2[14]) ||  //하미B";
                       (arr2[11] < arr2[9] && arr2[12] <= arr2[10] && arr2[13] >= arr2[11] && arr2[14] <= arr2[12] && arr2[15] >= arr2[13] && arr2[15] < arr2[14]) ||
                       (arr2[9] < arr2[7] && arr2[10] <= arr2[8] && arr2[11] >= arr2[9] && arr2[12] <= arr2[10] && arr2[13] >= arr2[11] && arr2[14] <= arr2[12] &&
                             arr2[15] >= arr2[13] && arr2[15] < arr2[14]) ||
                       (arr2[7] < arr2[5] && arr2[8] <= arr2[6] && arr2[9] >= arr2[7] && arr2[10] <= arr2[8] && arr2[11] >= arr2[9] && arr2[12] <= arr2[10] &&
                              arr2[13] >= arr2[11] && arr2[14] <= arr2[12] && arr2[15] >= arr2[13] && arr2[15] < arr2[14])))

                    c6 = "HM";   

                else
                    c6 = "0";  
             
            }
            else if (arr2[15] < arr2[14])
            {

                 if (
                  (arr2[15] < arr2[14] && arr2[15] < arr2[13] && arr2[14] <= arr2[12]) &&
                              (arr2[16] > arr2[14] ||
                              (arr2[16] <= arr2[14] && arr2[17] < 0.1) ||
                              (arr2[16] <= arr2[14] && arr2[17] >= arr2[15])))

                    c6 = "R3";  

                else if (
                    (arr2[14] > arr2[12] && arr2[15] >= arr2[13] && arr2[16] <= arr2[14] && arr2[16] > arr2[15] ||//상미A"
                    (arr2[12] > arr2[10] && arr2[13] >= arr2[11] && arr2[14] <= arr2[12] && arr2[15] >= arr2[13] && arr2[16] <= arr2[14] && arr2[16] > arr2[15]) ||
                    (arr2[10] > arr2[8] & arr2[11] >= arr2[9] && arr2[12] <= arr2[10] && arr2[13] >= arr2[11] && arr2[14] <= arr2[12] && arr2[15] >= arr2[13] &&
                        arr2[16] <= arr2[14] && arr2[16] > arr2[15]) ||
                    (arr2[8] > arr2[6] && arr2[9] >= arr2[7] && arr2[10] <= arr2[8] && arr2[11] >= arr2[9] && arr2[12] <= arr2[10] && arr2[13] >= arr2[11] &&
                        arr2[14] <= arr2[12] && arr2[15] >= arr2[13] && arr2[16] <= arr2[14] && arr2[16] > arr2[15]) ||
                    (arr2[13] > arr2[11] && arr2[14] >= arr2[12] & arr2[15] <= arr2[13] && arr2[15] > arr2[14]) || //상미B";
                    (arr2[11] > arr2[9] && arr2[12] >= arr2[10] && arr2[13] <= arr2[11] && arr2[14] >= arr2[12] && arr2[15] <= arr2[13] && arr2[15] > arr2[14]) ||
                    (arr2[9] > arr2[7] && arr2[10] >= arr2[8] && arr2[11] <= arr2[9] && arr2[12] >= arr2[10] && arr2[13] <= arr2[11] && arr2[14] >= arr2[12] &&
                        arr2[15] <= arr2[13] && arr2[15] > arr2[14]) ||
                    (arr2[7] > arr2[5] && arr2[8] >= arr2[6] && arr2[9] <= arr2[7] && arr2[10] >= arr2[8] && arr2[11] <= arr2[9] && arr2[12] >= arr2[10] &&
                        arr2[13] <= arr2[11] && arr2[14] >= arr2[12] && arr2[15] <= arr2[13] && arr2[15] > arr2[14])))

                    c6 = "SM"; 

                else if (
                       (arr2[14] < arr2[12] && arr2[15] <= arr2[13] && arr2[16] >= arr2[14] && arr2[16] < arr2[15] ||   //하미A";
                       (arr2[12] < arr2[10] && arr2[13] <= arr2[11] && arr2[14] >= arr2[12] && arr2[15] <= arr2[13] && arr2[16] >= arr2[14] && arr2[16] < arr2[15]) ||
                       (arr2[10] < arr2[8] && arr2[11] <= arr2[9] && arr2[12] >= arr2[10] && arr2[13] <= arr2[11] && arr2[14] >= arr2[12] && arr2[15] <= arr2[13] &&
                           arr2[16] >= arr2[14] && arr2[16] < arr2[15]) ||
                       (arr2[8] < arr2[6] && arr2[9] <= arr2[7] && arr2[10] >= arr2[8] && arr2[11] <= arr2[9] && arr2[12] >= arr2[10] && arr2[13] <= arr2[11] &&
                           arr2[14] >= arr2[12] && arr2[15] <= arr2[13] && arr2[16] >= arr2[14] && arr2[16] < arr2[15]) ||
                       (arr2[13] < arr2[11] && arr2[14] <= arr2[12] && arr2[15] >= arr2[13] && arr2[15] < arr2[14]) ||  //하미B";
                       (arr2[11] < arr2[9] && arr2[12] <= arr2[10] && arr2[13] >= arr2[11] && arr2[14] <= arr2[12] && arr2[15] >= arr2[13] && arr2[15] < arr2[14]) ||
                       (arr2[9] < arr2[7] && arr2[10] <= arr2[8] && arr2[11] >= arr2[9] && arr2[12] <= arr2[10] && arr2[13] >= arr2[11] && arr2[14] <= arr2[12] &&
                             arr2[15] >= arr2[13] && arr2[15] < arr2[14]) ||
                       (arr2[7] < arr2[5] && arr2[8] <= arr2[6] && arr2[9] >= arr2[7] && arr2[10] <= arr2[8] && arr2[11] >= arr2[9] && arr2[12] <= arr2[10] &&
                              arr2[13] >= arr2[11] && arr2[14] <= arr2[12] && arr2[15] >= arr2[13] && arr2[15] < arr2[14])))

                    c6 = "HM";  

                else
                    c6 = "0";   
            }
            return c6;
        }

        private string GetC7(string[] arr)
        {
            string c7 = "0";
            if (arr[0] == "0" && arr[1] == "SM" && arr[2] == "SM")
                c7 = arr[0];
            else if (arr[0] == "0" && arr[1] == "HM" && arr[2] == "HM")
                c7 = arr[0];
            else
            {
                //c7 = "0";
                //for (int i = 0; i < 10; i++)
                //{
                //    var tmp = 0;
                //    for (int c = 0; c < 9 - i; c++)
                //    {
                //        if (arr[c] != "0")
                //            tmp++;
                //    }

                //    if (tmp == 0)
                //    {
                //        if (i == 9 && arr[0] != "0")
                //            c7 = arr[0];
                //        else
                //            c7 = arr[9 - i];
                //        break;
                //    }
                //}

                #region process

                if (arr[0] == "0" && arr[1] == "0" && arr[2] == "0" && arr[3] == "0" && arr[4] == "0" &&
                     arr[5] == "0" && arr[6] == "0" && arr[7] == "0" && arr[8] == "0")
                    c7 = arr[9];
                else if (arr[0] == "0" && arr[1] == "0" && arr[2] == "0" && arr[3] == "0" && arr[4] == "0" &&
                     arr[5] == "0" && arr[6] == "0" && arr[7] == "0")
                    c7 = arr[8];
                else if (arr[0] == "0" && arr[1] == "0" && arr[2] == "0" && arr[3] == "0" && arr[4] == "0" &&
                        arr[5] == "0" && arr[6] == "0")
                    c7 = arr[7];
                else if (arr[0] == "0" && arr[1] == "0" && arr[2] == "0" && arr[3] == "0" && arr[4] == "0" && arr[5] == "0")
                    c7 = arr[6];
                else if (arr[0] == "0" && arr[1] == "0" && arr[2] == "0" && arr[3] == "0" && arr[4] == "0")
                    c7 = arr[5];
                else if (arr[0] == "0" && arr[1] == "0" && arr[2] == "0" && arr[3] == "0")
                    c7 = arr[4];
                else if (arr[0] == "0" && arr[1] == "0" && arr[2] == "0")
                    c7 = arr[3];
                else if (arr[0] == "0" && arr[1] == "0")
                    c7 = arr[2];
                else if (arr[0] == "0")
                    c7 = arr[1];
                else if (arr[0] != "0")
                    c7 = arr[0];
                else
                    c7 = "0";
                #endregion
            }

            return c7;
        }

        public Field_Step4[] ProcStep4(Field_Step3[] step3)
        {
            try
            {
                List<Field_Step4> lst = new List<Field_Step4>();

                int nRow = step3.Length;
                for (int r = 0; r < nRow; r++)
                {
                    int period = step3[r].c0;
                    if (period > 0)
                    {
                        for (int i = 0; i < period; i++)
                            lst.Add(step3[r]);
                    }
                }
                return lst.ToArray();
            }
            catch (Exception ex) { throw new Exception("step4 데이터가 없습니다.", ex); }
        }

        public Field_Step5[] ProcStep5(Field_Step4[] step4, int nMin)
        {
            try
            {
                int nRow = step4.Length;
                Field_Step5[] arr = new Field_Step5[nRow];
                DateTime dt_last = DateTime.MinValue;
                IFormatProvider provider = CultureInfo.InvariantCulture.DateTimeFormat;

                for (int r = 0; r < nRow; r++)
                {
                    #region color

                    string x = step4[r].c7;
                    if (string.IsNullOrEmpty(x) || x.Equals("0"))
                        arr[r].color = "무";
                    else
                        arr[r].color = step4[r].c7;

                    #endregion

                    #region date, value

                    arr[r].date = DateTime.ParseExact(step4[r].c3, "yyyy-MM-dd_HH:mm", provider);
                    arr[r].value = step4[r].c4;
                    arr[r].e = step4[r].c8;
                    arr[r].x = step4[r].c9;
                    double dx = step4[r].c10;
                    if (dx != 0.0)
                    {
                        DateTime dt = arr[r].date.Date;
                        //if (nMin <= 45) // 일일간격 (변동없음)
                        if (45 < nMin && nMin <= 90) // 주간간격
                        {
                            if (dt.DayOfWeek != dt_last.DayOfWeek)
                                dt_last = dt;
                            else dx = 0.0;
                        }
                        else if (nMin > 90)// 월간격
                        {
                            if (dt.Month != dt_last.Month)
                                dt_last = dt;
                            else dx = 0.0;
                        }
                    }
                    arr[r].dx = dx;

                    #endregion

                    #region ss

                    if (r + 1 < nRow)
                    {
                        if (step4[r].c5 == "aaa" && step4[r + 1].c5 == "aaa" && step4[r + 2].c5 != "aaa")
                        {
                            arr[r].ss = step4[r].c4;
                        }
                        else if (step4[r].c5 == "aaa" && step4[r - 1].c5 == "aaa" && step4[r + 1].c5 != "aaa")
                        {
                            arr[r].ss = 0.0;
                        }
                        else if (step4[r].c5 == "aaa2" && step4[r + 1].c5 == "aaa2" && step4[r + 2].c5 != "aaa2")
                        {
                            arr[r].ss = step4[r].c4;
                        }
                        else if (step4[r].c5 == "aaa2" && step4[r - 1].c5 == "aaa2" && step4[r + 1].c5 != "aaa2")
                        {
                            arr[r].ss = step4[r].c4;
                        }
                        else if (step4[r].c5 == "aab" && step4[r + 1].c5 == "aab" && step4[r + 2].c5 != "aab")
                        {
                            arr[r].ss = step4[r].c4;
                        }
                        else if (step4[r].c5 == "aab" && step4[r - 1].c5 == "aab" && step4[r + 1].c5 != "aab")
                        {
                            arr[r].ss = 0.0;
                        }
                        else if (step4[r].c5 == "aab2" && step4[r + 1].c5 == "aab2" && step4[r + 2].c5 != "aab2")
                        {
                            arr[r].ss = step4[r].c4;
                        }
                        else if (step4[r].c5 == "aab2" && step4[r - 1].c5 == "aab2" && step4[r + 1].c5 != "aab2")
                        {
                            arr[r].ss = step4[r].c4;
                        }
                        else if (step4[r].c5 == "bba" && step4[r + 1].c5 == "bba" && step4[r + 2].c5 != "bba")
                        {
                            arr[r].ss = step4[r].c4;
                        }
                        else if (step4[r].c5 == "bba" && step4[r - 1].c5 == "bba" && step4[r + 1].c5 != "bba")
                        {
                            arr[r].ss = 0.0;
                        }
                        else if (step4[r].c5 == "bbb" && step4[r + 1].c5 == "bbb" && step4[r + 2].c5 != "bbb")
                        {
                            arr[r].ss = step4[r].c4;
                        }
                        else if (step4[r].c5 == "bbb" && step4[r - 1].c5 == "bbb" && step4[r + 1].c5 != "bbb")
                        {
                            arr[r].ss = 0.0;
                        }
                        else if (step4[r].c5 == "ccc" && step4[r + 1].c5 == "ccc" && step4[r + 2].c5 != "ccc")
                        {
                            arr[r].ss = step4[r].c4;
                        }
                        else if (step4[r].c5 == "ccc" && step4[r - 1].c5 == "ccc" && step4[r + 1].c5 != "ccc")
                        {
                            arr[r].ss = 0.0;
                        }
                        else if (step4[r].c5 == "ddd" && step4[r + 1].c5 == "ddd" && step4[r + 2].c5 != "ddd")
                        {
                            arr[r].ss = step4[r].c4;
                        }
                        else if (step4[r].c5 == "ddd" && step4[r - 1].c5 == "ddd" && step4[r + 1].c5 != "ddd")
                        {
                            arr[r].ss = 0.0;
                        }
                        else if (step4[r].c5 != "0" && step4[r].c5 != step4[r + 1].c5)
                        {
                            arr[r].ss = step4[r].c4; //+ (item[4] * 0.001);
                        }
                        else
                            arr[r].ss = 0.0;
                    }
                    else arr[r].ss = 0.0;

                    #endregion
                }

                return arr;
            }
            catch (Exception ex) { throw new Exception("step5 데이터가 없습니다.", ex); }
        }

        public Field_Step3_1[] ProcStep3_1(Field_Step3[] step3)
        {
            try
            {
                int nRow = step3.Length;
                Field_Step3_1[] res = new Field_Step3_2[nRow];

                for (int r = 0; r < nRow; r++)
                {
                    if (step3[r].c5 == "aaa" && step3[r - 1].c5 == "aaa")
                        res[r].c3 = 0.0;
                    else if (step3[r].c5 == "aab" && step3[r - 1].c5 == "aab")
                        res[r].c3 = 0.0;
                    else if (step3[r].c5 == "bba" && step3[r - 1].c5 == "bba")
                        res[r].c3 = 0.0;
                    else if (step3[r].c5 == "bbb" && step3[r - 1].c5 == "bbb")
                        res[r].c3 = 0.0;
                    else if (step3[r].c5 != "0")
                        res[r].c3 = step3[r].c4;
                    else
                        res[r].c3 = 0.0;

                    res[r].c2 = step3[r].c3;

                    if (r >= (15 - 1))
                    {
                        #region c1

                        double[] arr = new double[15];
                        for (int j = 0; j < 15; j++)
                        {
                            try
                            {
                                int i = r - 14 + j;
                                if (i < 0)
                                    arr[j] = 0;
                                else
                                    arr[j] = res[i].c3;
                            }
                            catch { arr[j] = 0; }
                        }

                        // process
                        arr = arr.Reverse().ToArray();

                        res[r].c0 = 0;
                        for (int i = 0; i < 15; i++)
                        {
                            var tmpSum = arr.Skip(1).Take(14 - i).Sum();
                            if (arr[0] > 0 && tmpSum == 0)
                            {
                                res[r].c0 = 15 - i;
                                break;
                            }
                        }

                        #endregion

                        #region c2

                        //IF(BY18=1,1,IF(BY18=3,2,IF(BY18=5,3,IF(BY18=7,4,IF(BY18=9,5,IF(BY18=11,6,IF(BY18=13,7,IF(BY18=15,8,IF(BY18=17,9,BY18)))))))))
                        int c1 = res[r].c0;
                        int c2 = c1;
                        switch (c1)
                        {
                            case 1: c2 = 1; break;
                            case 3: c2 = 2; break;
                            case 5: c2 = 3; break;
                            case 7: c2 = 4; break;
                            case 9: c2 = 5; break;
                            case 11: c2 = 6; break;
                            case 13: c2 = 7; break;
                            case 15: c2 = 8; break;
                            case 17: c2 = 9; break;
                        }
                        res[r].c1 = c2;

                        //res[r][1] = res[r][0];
                        //for (int i = 1; i < 18; i += 2)
                        //{
                        //    if (res[r][0] == i)
                        //    {
                        //        res[r][1] = i;
                        //        break;
                        //    }
                        //}

                        #endregion
                    }
                    else { res[r].c0 = 0; res[r].c1 = 0; }

                    res[r].외 = step3[r].c1;

                }
                return res;
            }
            catch (Exception ex) { throw new Exception("step3_1 데이터가 없습니다.", ex); }
        }

        public Field_Step3_2[] ProcStep3_2(Field_Step4_1[] step4_1)
        {
            try
            {
                int nRow = step4_1.Length;
                Field_Step3_2[] res = new Field_Step3_2[nRow];
                for (int r = 0; r < nRow; r++)
                {
                    if (step4_1[r].c3 != "0")
                        res[r].c3 = step4_1[r].c2;
                    else
                        res[r].c3 = 0.0;

                    res[r].c2 = step4_1[r].c1;
                    //IF(BY9=1,1,IF(BY9=3,2,IF(BY9=5,3,IF(BY9=7,4,IF(BY9=9,5,IF(BY9=11,6,IF(BY9=13,7,IF(BY9=15,8,IF(BY9=17,9,BY9)))))))))

                    if (r >= (15 - 1))
                    {
                        #region c1

                        double[] arr = new double[15];
                        for (int j = 0; j < 15; j++)
                        {
                            try
                            {
                                var i = r - 14 + j;
                                if (i < 0)
                                    arr[j] = 0;
                                else
                                    arr[j] = res[i].c3;
                            }
                            catch { arr[j] = 0; }
                        }

                        // process
                        arr = arr.Reverse().ToArray();
                        res[r].c0 = 0;
                        for (int i = 0; i < 15; i++)
                        {
                            var tmpSum = arr.Skip(1).Take(14 - i).Sum();
                            if (arr[0] > 0 && tmpSum == 0)
                            {
                                res[r].c0 = 15 - i;
                                break;
                            }
                        }

                        #endregion

                        #region c2

                        //IF(BY18=1,1,IF(BY18=3,2,IF(BY18=5,3,IF(BY18=7,4,IF(BY18=9,5,IF(BY18=11,6,IF(BY18=13,7,IF(BY18=15,8,IF(BY18=17,9,BY18)))))))))
                        int c1 = res[r].c0;
                        int c2 = c1;
                        switch (c1)
                        {
                            case 1: c2 = 1; break;
                            case 3: c2 = 2; break;
                            case 5: c2 = 3; break;
                            case 7: c2 = 4; break;
                            case 9: c2 = 5; break;
                            case 11: c2 = 6; break;
                            case 13: c2 = 7; break;
                            case 15: c2 = 8; break;
                            case 17: c2 = 9; break;
                        }
                        res[r].c1 = c2;
                        //res[r][1] = res[r][0];
                        //for (int i = 1; i < 18; i += 2)
                        //{
                        //    if (res[r][0] == i)
                        //    {
                        //        res[r][1] = i;
                        //        break;
                        //    }
                        //}

                        #endregion
                    }
                    else { res[r].c0 = 0; res[r].c1 = 0; }

                    res[r].외 = step4_1[r].외;

                }
                return res;
            }
            catch (Exception ex) { throw new Exception("step3_2 데이터가 없습니다.", ex); }
        }

        public Field_Step4_1[] ProcStep4_1(Field_Step3_1[] step3_1, int idx_chart)
        {
            try
            {
                List<Field_Step4_1> lst = new List<Field_Step4_1>();

                int nRow = step3_1.Length;
                for (int r = 0; r < nRow; r++)
                {
                    if (step3_1[r].c3 == 0.0)
                        continue;

                    Field_Step4_1 row = new Field_Step4_1();
                    row.c0 = step3_1[r].c1;
                    row.c1 = step3_1[r].c2;
                    row.c2 = step3_1[r].c3;
                    row.외 = step3_1[r].외;
                    lst.Add(row);
                }

                Field_Step4_1[] res = lst.ToArray();
                nRow = res.Length;
                for (int r = 0; r < nRow; r++)
                {
                    // c5
                    #region c5

                    double[] arr = new double[23]; //21
                    for (int j = 0; j < 23; j++)
                    {
                        int i = r - 5 + j;
                        if (i < 0 || i > nRow - 1)
                            arr[j] = 0;
                        else
                            arr[j] = res[i].c2;
                    }
                    res[r].c3 = GetC5(arr, res[r].외, idx_chart);

                    #endregion

                    #region c6
                    // c6
                    double[] arr2 = new double[19];
                    for (int j = 0; j < 19; j++)
                    {
                        int i = r - 15 + j;
                        if (i < 0 || i > nRow - 1)
                            arr2[j] = 0;
                        else
                            arr2[j] = res[i].c2;
                    }
                    res[r].c4 = GetC6(arr2);
                    #endregion
                }

                #region c7

                for (int r = 0; r < nRow; r++)
                {
                    #region c7 process

                    string[] arr = new string[10];
                    for (int j = 0; j < 10; j++)
                    {
                        int i = r + j;
                        if (i < 0 || i > nRow - 1)
                            arr[j] = "0";
                        else
                            arr[j] = res[i].c4;
                    }
                    res[r].c5 = GetC7(arr);
                    #endregion
                }

                #endregion

                return res;
            }
            catch (Exception ex) { throw new Exception("step4_1 데이터가 없습니다.", ex); }
        }

        public Field_Step5_1[] ProcStep5_1(Field_Step4_1[] step4_1)
        {
            try
            {
                List<Field_Step5_1> lst = new List<Field_Step5_1>();
                int nRow = step4_1.Length;
                for (int r = 0; r < nRow; r++)
                {
                    int cnt = step4_1[r].c0;
                    for (int i = 0; i < cnt; i++)
                        lst.Add(step4_1[r]);

                }
                return lst.ToArray();
            }
            catch (Exception ex) { throw new Exception("step5_1 데이터가 없습니다.", ex); }
        }

        public Field_Step6_1[] ProcStep6_1(Field_Step5_1[] step5_1)
        {
            try
            {
                int nRow = step5_1.Length;

                IFormatProvider provider = CultureInfo.InvariantCulture.DateTimeFormat;
                Field_Step6_1[] res = new Field_Step6_1[nRow];
                for (int r = 0; r < nRow; r++)
                {
                    string x = step5_1[r].c5;
                    if (string.IsNullOrEmpty(x) || x.Equals("0"))
                        res[r].color = "무";
                    else
                        res[r].color = step5_1[r].c5;

                    res[r].date = DateTime.ParseExact(step5_1[r].c1, "yyyy-MM-dd_HH:mm", provider);
                    res[r].value = step5_1[r].c2;

                    if (r >= nRow - 1)
                    {
                        res[r].ss = 0.0;
                        break;
                    }

                    if (r + 1 < nRow)
                    {
                        if (step5_1[r].c3 != "0" &&
                            step5_1[r].c3 != step5_1[r + 1].c3)
                            res[r].ss = step5_1[r].c2;// + 0.3;
                        else
                            res[r].ss = 0.0;
                    }
                    else res[r].ss = 0.0;
                }
                return res;
            }
            catch (Exception ex) { throw new Exception("step6 데이터가 없습니다.", ex); }
        }

        #endregion
    }
}
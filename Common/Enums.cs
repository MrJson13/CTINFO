using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Enums
    {
        public Enums()
        { }
        /// <summary>
        /// 学历枚举
        /// </summary>
        public enum Education
        {
            [Description("高中及以下")]
            高中及以下 = 1,
            [Description("中专")]
            中专 = 2,
            [Description("大专")]
            大专 = 3,
            [Description("本科")]
            本科 = 4,
            [Description("硕士")]
            硕士 = 5,
            [Description("博士")]
            博士 = 6,
            [Description("其他")]
            其他 = 7
        }

        public enum SocialSecurity
        {
            连续购买时间 = 1,
            累计购买时间 = 2,
            当前公司购买时间 = 3
        }
        public enum RemindType
        {
            劳动合同提醒 = 1,
            证书到期提醒 = 2,
            转正提醒 = 3,
            生日提醒 = 4,
            退休提醒 = 5,
            流程处理提醒 = 6,
            //租房合同提醒 = 7,
            //房租到期提醒 = 8,
            //资质提醒 = 9,
            //资质年检提醒 = 10,
            身份证提醒 = 11,
            车辆年检提醒 = 12,
            车辆保险到期提醒 = 13,
            //工作提醒 = 14,
            //会议纪要提醒 = 15,
            证书更新提醒 = 16,
            //工作知会提醒 = 17,
            面试提醒 = 18,
            收文提醒 = 19,
            流程其他提醒 = 20,
            发文提醒 = 21,
        }
        public enum FlowID
        {
            印章使用 = 1,
            承揽合同 = 2,
            招聘申请 = 3,
            证照使用 = 4,
            信息报送 = 5,
            岗位调整 = 6,
            离职申请 = 7,
            发文流程 = 8,
            收文流程 = 9,
            档案借阅 = 10,
            工会入会申请 = 11,
            信访流程 = 12,
            培训计划 = 13,
            采购申请 = 14,
            保证金=20,
            供方管理 = 19,
            部门通知 = 48,
            开票 = 44
        }

        public static string GetEnumDescription(Enum enumValue)
        {
            string value = enumValue.ToString();
            FieldInfo field = enumValue.GetType().GetField(value);
            object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);    //获取描述属性
            if (objs == null || objs.Length == 0)    //当描述属性没有时，直接返回名称
                return value;
            DescriptionAttribute descriptionAttribute = (DescriptionAttribute)objs[0];
            return descriptionAttribute.Description;
        }
        /// <summary>
        /// 在职状态
        /// </summary>
        public enum EmploymentState
        {
            回收站 = -1,
            在职 = 1,
            停薪留职 = 2,
            退休 = 3,
            离职 = 4

        }

        /// <summary>
        /// 保证金类型
        /// </summary>
        public enum Margin
        {
            投标保证金 = 1,
            履约保证金 = 2
        }
        /// <summary>
        /// 保证金付款类型
        /// </summary>
        public enum MarginPaymentType
        {
            中道明华建设工程项目咨询有限责任公司 = 1,
            四川明宸工程项目管理有限公司 = 3,
            四川明镜会计事务所 = 2//如果是这个后面加【（普通合伙）】
        }

        /// <summary>
        /// 用于记录操作日志的Type
        /// </summary>
        public enum MenuType
        {
            宿舍 = 1,
            宿舍人员 = 2,
            获奖信息 = 3,
            合同到期提醒 = 4,
            证书到期提醒 = 5,
            转正提醒 = 6,
            生日提醒 = 7,
            退休提醒 = 8,
            身份证提醒 = 9,
            人员基本信息 = 10,
            教育经历 = 11,
            证书信息 = 12,
            岗位变动 = 13,
            劳动合同 = 14,
            社保信息 = 15,
            离职信息 = 16,
            家庭关系 = 17,
            工作经历 = 18,
            项目经验 = 19,
            培训经历 = 20,
            招聘活动 = 21,
            会议纪要管理 = 22,
            证照管理 = 23,
            资质类型 = 24,
            资质提醒配置 = 25,
            常用网址 = 26,
            知识库 = 27,
            人员管理 = 28,
            资质工作记录 = 29,
            资产管理 = 30,
            车辆管理 = 31,
            发文 = 32,
            收文 = 33,
            档案管理 = 34,
            考试题库管理 = 35,
            供方管理 = 36,
            综合事务管理 = 37,
            材料设备周材合同管理 = 38,           
            招采管理 = 39,
            机料合同清单计价 = 40,
            承揽合同 = 41,
            劳务合同清单计价 = 42,
            部门通知 = 43,
            在建项目 = 44,
            劳务专业分包合同 = 45,
            调差管理 = 46,
            变更管理 = 47
        }
        /// <summary>
        /// 用于记录操作日志的Type
        /// </summary>
        public enum OperateType
        {
            添加 = 0,
            编辑 = 1,
            删除 = 2,
            查询 = 3,
            上传附件 = 4,          
            其他 = 6
        }

        /// <summary>
        /// 供方评定等级
        /// </summary>
        public enum EvaluateType
        {
            A级 = 1,
            B级 = 2,
            C级 = 3,
            D级 = 4
        }

        /// <summary>
        /// 招标形式
        /// </summary>
        public enum BiddingPurchaseForm
        {
            公开招标 = 1,
            邀请招标 = 2,
            直接发包 = 3,
            询价 = 4,
            单一来源采购 = 5
        }

        public enum SupplierType
        {
            劳务分包 = 1,
            专业分包 = 2,
            材料供应 = 3,
            设备租赁 = 4,
            周转材料租赁 = 5,
            服务=6
        }
        
        /// <summary>
        /// 文件类型,默认与FlowID相同，如果同一个流程下有多重文件类型，则从1000开始编号
        /// </summary>
        public enum FileType
        {
            劳务专业分包合同=1,


            /**上面是流程相关的，下面是其他**/

            供方信息附件 = 10000,
            合同管理附件=10001,
            在建项目=10002,
            项目管理附件 = 10003,
            招采信息 = 10004,
            调差信息 = 10005,
            变更信息 = 10006,

            投资估算 = 10007,
            全周期 = 10008,
            初步设计概算 = 10009,
            施工图预算 = 10010,
            目标成本 = 10011,
            //本季度发生成本 = 10012,
            预计剩余发生成本金额 = 10013,
            预估总成本 = 10014,
            动态成本分析 = 10015,
            结算 = 10016,
            决算 = 10017,
            原合同清单计量金额 = 10018,
            已批复的变更计量金额 = 10019,
            子项目清单及估概预算批复情况 = 10020

        }
        /// <summary>
        /// 权限类型,默认与FlowID相同，如果没有流程，则自定义一个
        /// </summary>
        public enum PowerType
        {
            发文 = 8,
            收文 = 9,           
            部门通知 = 48
        }
        /// <summary>
        /// 律师审批类型
        /// </summary>
        public enum ApproveType
        {
            承揽合同 = 1,
            劳务专业分包合同 = 2,
            机料合同 = 3
        }
        /// <summary>
        /// 印章类型
        /// </summary>
        public enum SealType
        {
            合同章 = 1,
            法人章 = 2,
            公司公章 = 3,
            工会公章 = 5,
            党支部公章 = 6,
            工会主席章 = 7,
            法人签字 =4,
            项目公章 = 8

        }
        
        /// <summary>
        /// 入库级别
        /// </summary>
        public enum SupplierClass {
            公司级=1,
            项目级=2
        }
        /// <summary>
        /// 动态成本分析阶段
        /// </summary>
        public enum DynamicCostPhase {
            投资估算 = 1,
            全周期 = 2,
            初步设计概算 = 3,
            施工图预算 = 4,
            目标成本 = 5,
            本季度发生成本 = 6,
            累计发生成本 = 7,
            预计剩余发生成本金额 = 9,
            预估总成本 = 10,
            动态成本分析 = 11,
            结算 = 12,
            决算 = 13
        }
        /// <summary>
        /// 本季度发生成本分析阶段
        /// </summary>
        public enum DynamicCostQuarterPhase
        {
            原合同清单计量金额 = 1,
            已批复的变更计量金额 = 2
        }
        /// <summary>
        /// 调差类型
        /// </summary>
        public enum EnumAdjustDifType
        {
            人工费调差 = 1,
            材料费调差 = 2
        }
        /// <summary>
        /// 变更类型
        /// </summary>
        public enum EnumAlterationType
        {
             设计变更 = 1,
             技术核定洽商 = 2,
             技术经济签证 = 3,
             索赔 = 4
        }
    }
}

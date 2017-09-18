//===============================================================================
//
//	LabLed
//  ��� --  LED��ʾ�� ���  
//  
//   ----- ���ڶԻ���LED��ʾ������
//         
//===============================================================================
//
// Copyright (C) 2002-2007 ��������ʱ�����������
// �������е�Ȩ��.
// 
// ��������: 2007-12-10
// �� �� ��: Liushiying (lsy@sogou.com)
// ����˵��: LED��ʾ��������
// �޸�����: 2007-12-10
// �� �� ��: Liushiying (lsy@sogou.com)
// �޸�˵��:  
//
//===============================================================================


using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
/// <summary>
/// LED��ʾ��������
/// </summary>
public class LedCommon : IDisposable
{
    #region ������ö�ٶ���
    /// <summary>
    /// ͨѶ��ʽ����
    /// </summary>
    public enum eDevType
    {
        DEV_COM = 0,                //����ͨѶ    0
        DEV_UDP,	                //UDPͨѶ     1
        DEV_MODEM		            //ModemͨѶ   2
    }
    /// <summary>
    /// �˿ڳ���
    /// </summary>
    public enum eCommPort
    {
        Address = 0,                //�����ַ
        ComPort = 4                //��COM4��ΪͨѶ�˿�
    }

    /// <summary>
    /// ����ͨѶ�ٶ�
    /// </summary>
    public enum eBaudRate
    {
        SBR_9600 = 0,               //��������9600   0
        SBR_14400,                  //��������14400  1
        SBR_19200,                  //��������19200  2
        SBR_38400,                  //��������38400  3
        SBR_57600,                  //��������57600  4
        SBR_115200                  //��������115200 5
    }

    /// <summary>
    /// ��������
    /// </summary>
    public enum eRootType
    {
        ROOT_PLAY = 17,             //�·���ĿΪ�������� 17
        ROOT_DOWNLOAD				//�·���ĿΪ���沢���� 18
    }

    /// <summary>
    /// ��ʾ�����ͳ���
    /// </summary>
    public enum eScreenType
    {
        SCREEN_UNICOLOR = 1,		//��ɫ��ʾ��
        SCREEN_COLOR,			//˫ɫ��ʾ��
        SCREEN_RGB,			//�޻Ҷ�ȫ��
        SCREEN_FULL_4BIT,		//16���Ҷ�ȫ��
        SCREEN_FULL_10BIT		//1024���Ҷ�ȫ��	
    }

    /// <summary>
    /// ��Ӧ��Ϣ����
    /// </summary>
    public enum eResponseMessage
    {
        LM_RX_COMPLETE = 1,          //���ս��� 1
        LM_TX_COMPLETE,		         //���ͽ��� 2
        LM_RESPOND,		             //�յ�Ӧ�� 3
        LM_TIMEOUT,		             //��ʱ 4
        LM_NOTIFY,		             //֪ͨ��Ϣ 5
        LM_PARAM,
        LM_TX_PROGRESS,		         //������ 7
        LM_RX_PROGRESS		         //������ 8
    }

    /// <summary>
    /// ��Դ״̬����
    /// </summary>
    public enum ePowerStatus
    {
        LED_POWER_OFF = 0,          //��ʾ����Դ�ѹر� 0
        LED_POWER_ON	            //��ʾ����Դ�� 1
    }

    //ʱ���ʽ����
    public enum eTimeFormat
    {
        DF_YMD = 1,                 //  1 ������  "2004��12��31��"
        DF_HN,		                //  2 ʱ��    "19:20"
        DF_HNS,		                //  3 ʱ����  "19:20:30"
        DF_Y,		                //  4 ��      "2004"
        DF_M,		                //  5 ��      "12" "01" ע�⣺ʼ����ʾ��λ����
        DF_D,				        //  6 ��
        DF_H,	                    //  7 ʱ
        DF_N,			            //  8 ��
        DF_S,		                //  9 ��
        DF_W		                // 10 ����    "������"
    }

    /// <summary>
    /// ����ʱ������ʱformat����
    /// </summary>
    public enum eCountType
    {
        CF_DAY = 0,                 // 0 ����
        CF_HOUR,					// 1 Сʱ��
        CF_HMS,						// 2 ʱ����
        CF_HM,						// 3 ʱ��
        CF_MS,						// 4 ����
        CF_S						// 5 ��
    }

    public const int ETHER_ADDRESS_LENGTH = 6;
    public const int IP_ADDRESS_LENGTH = 4;

    public const int NOTIFY_NONE = 0;
    public const int NOTIFY_EVENT = 1;
    public const int NOTIFY_BLOCK = 2;

    public const int FONT_SET_16 = 0;            //16�����ַ�
    public const int FONT_SET_24 = 1;            //24�����ַ�

    public const int PKC_QUERY = 4;
    public const int PKC_ADJUST_TIME = 6;
    public const int PKC_GET_PARAM = 7;
    public const int PKC_SET_PARAM = 8;
    public const int PKC_GET_POWER = 9;
    public const int PKC_SET_POWER = 10;
    public const int PKC_GET_BRIGHT = 11;
    public const int PKC_SET_BRIGHT = 12;
    public const int PKC_SET_AUTO_POWER = 61;
    public const int PKC_GET_LEAF = 65;
    public const int PKC_SET_LEAF = 66;
    #endregion

    #region �ṹ�嶨��
    /// <summary>
    /// �ṹ�嶨��
    /// </summary>

    //�豸����
    public struct DEVICEPARAM
    {
        public uint devType;                      //ͨѶ�豸����
        public uint Speed;                        //ͨѶ�ٶ�(���Դ���ͨѶ����)
        public uint ComPort;
        public uint FlowCon;
        public uint locPort;                      //���ض˿�(�Դ���ͨѶΪ�����ںţ���UDPͨѶΪ�����ض˿ںţ�һ��Ҫ����1024)
        public uint rmtPort;                      //Զ�̶˿ں�(��UDPͨѶ���ã�����Ϊ6666)
        public uint memory;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] Phone;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public uint[] Reserved;
    }

    //��ʾ������
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    public struct SYSTEMTIME
    {
        public ushort wYear;
        public ushort wMonth;
        public ushort wDayOfWeek;
        public ushort wDay;
        public ushort wHour;
        public ushort wMinute;
        public ushort wSecond;
        public ushort wMilliseconds;
    }

    public struct TIMESTAMP
    {
        public int time;
        public int date;
    }

    public struct AUTOPOWERTIME
    {
        public uint enabled;
        public int opentime0_time;
        public int opentime0_date;
        public int opentime1_time;
        public int opentime1_date;
        public int opentime2_time;
        public int opentime2_date;
        public int opentime3_time;
        public int opentime3_date;
        public int opentime4_time;
        public int opentime4_date;
        public int opentime5_time;
        public int opentime5_date;
        public int opentime6_time;
        public int opentime6_date;
        public int closetime0_time;
        public int closetime0_date;
        public int closetime1_time;
        public int closetime1_date;
        public int closetime2_time;
        public int closetime2_date;
        public int closetime3_time;
        public int closetime3_date;
        public int closetime4_time;
        public int closetime4_date;
        public int closetime5_time;
        public int closetime5_date;
        public int closetime6_time;
        public int closetime6_date;
    }

    public struct NotifyMessage
    {
        public int Message;
        public int Command;
        public int Result;
        public int Status;
        public int Address;
        public int Size;
        public int Buffer;
        public DEVICEPARAM param;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string Host;
        public int port;
    }

    public struct TDevInfo
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public char[] devName;  //�豸����
        public UInt32 devID;    //�豸��ʶ
        public UInt32 devIP;    //�豸IP��ַ
        public UInt16 devAddr;  //�豸��ַ
        public UInt16 devPort;  //�豸�˿�
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public UInt32[] reserved;
    }

    public struct TDevReport
    {
        public TDevInfo devInfo;
        public double timeUpdate;
    }

    public struct TBoardParam
    {
        public UInt16 width;
        public UInt16 height;
        public UInt16 type;
        public UInt16 frequency;
        public UInt32 flag;
        public UInt32 uart;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = ETHER_ADDRESS_LENGTH)]
        public byte[] mac;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = IP_ADDRESS_LENGTH)]
        public byte[] ip;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = ETHER_ADDRESS_LENGTH)]
        public byte[] GateMAC;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = IP_ADDRESS_LENGTH)]
        public byte[] host;
        public UInt32 brightness;
        public UInt32 rom_size;
        public Int32 left;
        public Int32 top;
        public UInt16 scan_mode;
        public UInt16 remote_port;
        public UInt16 line_order;
        public UInt16 oe_time;
        public UInt16 shift_freq;
        public UInt16 refresh_freq;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = IP_ADDRESS_LENGTH)]
        public byte[] GateIP;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = IP_ADDRESS_LENGTH)]
        public byte[] ipMask;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] name;
        public UInt32 ident;
        public UInt32 address;
    }

    #endregion

    /// <summary>
    /// API���
    /// </summary>

    public LedCommon()
    {
        //
        // TODO: �ڴ˴���ӹ��캯���߼�
        //
    }

    #region ��ȡ��ǰϵͳʱ��
    /// <summary>
    /// ��ȡ��ǰϵͳʱ�䣬����ʱ���ʽ
    /// </summary>
    [DllImport("kernel32.dll", EntryPoint = "GetLocalTime")]
    public static extern void DLL_GetLocalTime(ref SYSTEMTIME lpSystemTime);
    /// <summary>
    /// ��ȡ��ǰϵͳʱ��
    /// </summary>
    /// <param name="lpSystemTime">ϵͳʱ�����</param>
    /// <returns></returns>
    public void GetLocalTime(ref SYSTEMTIME lpSystemTime)
    {
        DLL_GetLocalTime(ref lpSystemTime);
    }
    #endregion

    #region ���� Dispose �ͷ���Դ
    /// <summary>
    /// �ͷ���Դ
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(true);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
            return;
    }
    #endregion

    #region ��ͨѶ�ŵ�
    //long (_stdcall *LED_Open)(const PDeviceParam param, long Notify, long Window, long Message);
    [DllImport("LEDSender.DLL", EntryPoint = "LED_Open")]
    public static extern int DLL_LED_Open(ref DEVICEPARAM param, int Notify, int Window, int Message);
    /// <summary>
    /// ��ͨѶ�ŵ�
    /// </summary>
    /// <param name="param">DEVICEPARAM�ṹ���豸����</param>
    /// <param name="Notify">�Ƿ����֪ͨ��Ϣ 0:������ 1:����</param>
    /// <param name="Window">����֪ͨ��Ϣ�Ĵ��ھ��</param>
    /// <param name="Message">��Ϣ��ʶ</param>
    /// <returns></returns>
    public int LED_Open(ref DEVICEPARAM param, int Notify, int Window, int Message)
    {
        return DLL_LED_Open(ref param, Notify, Window, Message);
    }
    #endregion

    #region �ر�ͨѶ�ŵ�

    //void (_stdcall *LED_Close)(long dev);
    [DllImport("LEDSender.DLL", EntryPoint = "LED_Close")]
    public static extern void DLL_LED_Close(int dev);
    /// <summary>
    /// �ر��Ѵ򿪵�ͨѶ�豸
    /// </summary>
    /// <param name="dev">LED_Open�����ķ���ֵ</param>
    public void LED_Close(int dev)
    {
        System.Threading.Thread.Sleep(100);

        DLL_LED_Close(dev);

    }
    #endregion

    #region �ر�����ͨѶ�ŵ�

    //void (_stdcall *LED_CloseAll)();
    [DllImport("LEDSender.DLL", EntryPoint = "LED_CloseAll")]
    public static extern void DLL_LED_CloseAll();
    /// <summary>
    /// �ر��Ѵ򿪵�ͨѶ�豸
    /// </summary>
    public void LED_CloseAll()
    {
        System.Threading.Thread.Sleep(100);
        DLL_LED_CloseAll();
    }
    #endregion

    #region ��ѯĳ��ͨѶ�ŵ���״̬

    //long (_stdcall *LED_GetDeviceStatus)(long dev);
    [DllImport("LEDSender.DLL", EntryPoint = "LED_GetDeviceStatus")]
    public static extern int DLL_LED_GetDeviceStatus(int dev);
    /// <summary>
    /// ��ѯĳ��ͨѶ�ŵ���״̬
    /// </summary>
    /// <param name="dev">LED_Open�����ķ���ֵ</param>
    /// <return>ͨѶ״̬ 0:���ŵ����У�����ͨѶ 1:�ŵ�����ͨѶ�� -1:�ŵ�δ�򿪻��ߴ򿪴���</return>
    public int LED_GetDeviceStatus(int dev)
    {
        return DLL_LED_GetDeviceStatus(dev);
    }
    #endregion

    #region �������߼�����ʾ������
    //void (_stdcall *LED_CreateReportServer)(WORD port);
    [DllImport("LEDSender.DLL", EntryPoint = "LED_CreateReportServer")]
    public static extern void DLL_LED_CreateReportServer(UInt16 port);
    /// <summary>
    /// �������߼�����ʾ������
    /// </summary>
    public void LED_CreateReportServer(UInt16 port)
    {
        DLL_LED_CreateReportServer(port);
    }
    #endregion

    #region �ͷ����߼�����ʾ������
    //void (_stdcall *LED_ReleaseReportServer)(void);
    [DllImport("LEDSender.DLL", EntryPoint = "LED_ReleaseReportServer")]
    public static extern void DLL_LED_ReleaseReportServer();
    /// <summary>
    /// �ͷ����߼�����ʾ������
    /// </summary>
    public void LED_ReleaseReportServer()
    {
        DLL_LED_ReleaseReportServer();
    }
    #endregion

    #region ��ȡ������ʾ������
    //long (_stdcall *LED_GetOnlineCount)(void);
    [DllImport("LEDSender.DLL", EntryPoint = "LED_GetOnlineCount")]
    public static extern int DLL_LED_GetOnlineCount();
    /// <summary>
    /// ��ȡ������ʾ������
    /// </summary>
    public int LED_GetOnlineCount()
    {
        return DLL_LED_GetOnlineCount();
    }
    #endregion

    #region ��ȡ������ʾ���б�
    //long (_stdcall *LED_GetOnlineList2)(PDevice* obuffer);
    [DllImport("LEDSender.DLL", EntryPoint = "LED_GetOnlineList2")]
    public static extern int DLL_LED_GetOnlineList(ref TDevReport obuffer);
    /// <summary>
    /// ��ȡ������ʾ���б�
    /// </summary>
    public int LED_GetOnlineList(ref TDevReport obuffer)
    {
        return DLL_LED_GetOnlineList(ref obuffer);
    }
    #endregion

    #region ��ȡ������ʾ��
    //long (_stdcall *LED_GetOnlineItem2)(long index, PDevice* obuffer);
    [DllImport("LEDSender.DLL", EntryPoint = "LED_GetOnlineItem2")]
    public static extern int DLL_LED_GetOnlineItem(int index, ref TDevReport obuffer);
    /// <summary>
    /// ��ȡ������ʾ���б�
    /// </summary>
    public int LED_GetOnlineItem(int index, ref TDevReport obuffer)
    {
        return DLL_LED_GetOnlineItem(index, ref obuffer);
    }
    #endregion

    #region ��ȡ������ʾ������
    //long (_stdcall *LED_GetDeviceOnlineCount)(long dev);
    [DllImport("LEDSender.DLL", EntryPoint = "LED_GetDeviceOnlineCount")]
    public static extern int DLL_LED_GetDeviceOnlineCount(int dev);
    /// <summary>
    /// ��ȡ������ʾ������
    /// </summary>
    public int LED_GetDeviceOnlineCount(int dev)
    {
        return DLL_LED_GetDeviceOnlineCount(dev);
    }
    #endregion

    #region ��ȡ������ʾ��
    //long (_stdcall *LED_GetDeviceOnlineItem2)(long dev, long index, PDevice* obuffer);
    [DllImport("LEDSender.DLL", EntryPoint = "LED_GetDeviceOnlineItem2")]
    public static extern int DLL_LED_GetDeviceOnlineItem(int dev, int index, ref TDevReport obuffer);
    /// <summary>
    /// ��ȡ������ʾ���б�
    /// </summary>
    public int LED_GetDeviceOnlineItem(int dev, int index, ref TDevReport obuffer)
    {
        return DLL_LED_GetDeviceOnlineItem(dev, index, ref obuffer);
    }
    #endregion

    #region ��ѯ��ʾ���Ƿ��ܹ�ͨѶ
    //void (_stdcall *LED_Query)(long dev, BYTE Address, char *Host, WORD port);
    [DllImport("LEDSender.DLL", EntryPoint = "LED_Query")]
    public static extern void DLL_LED_Query(int dev, byte Address, string Host, int port);
    /// <summary>
    /// ��ѯ��ʾ���Ƿ��ܹ�ͨѶ
    /// </summary>
    /// <param name="dev">�ò�����LED_Open�����ķ���ֵ</param>
    /// <param name="Address"></param>
    /// <param name="Host">��ʾ��IP��ַ (����UDP��Ч);����ͨѶ����д�����ַ���߿�</param>
    /// <param name="port">��ʾ���˿ں�(�����UDPͨѶ���ö˿�Ϊ6666)</param>
    public void LED_Query(int dev, byte Address, string Host, int port)
    {
        DLL_LED_Query(dev, Address, Host, port);
    }
    #endregion

    #region �ü����ʱ��У����ʾ����ʱ��
    //void (_stdcall *LED_AdjustTime)(long dev, BYTE Address, char *Host, WORD port);
    [DllImport("LEDSender.DLL", EntryPoint = "LED_AdjustTime")]
    public static extern void DLL_LED_AdjustTime(int dev, byte Address, string Host, int port);
    /// <summary>
    /// �����ʱ��У����ʾ����ʱ��
    /// </summary>
    /// <param name="dev"></param>
    /// <param name="Address"></param>
    /// <param name="Host"></param>
    /// <param name="port"></param>
    public void LED_AdjustTime(int dev, byte Address, string Host, int port)
    {
        DLL_LED_AdjustTime(dev, Address, Host, port);
    }
    #endregion

    #region �����Զ�������ʱ��
    //void (_stdcall *LED_SetAutoPowerList)(long dev, BYTE Address, char *Host, long port, PAutoPowerTime value);
    [DllImport("LEDSender.DLL", EntryPoint = "LED_SetAutoPowerList")]
    public static extern void DLL_LED_SetAutoPowerList(int dev, byte Address, string Host, int port, ref AUTOPOWERTIME value);
    /// <summary>
    /// �����ʱ��У����ʾ����ʱ��
    /// </summary>
    /// <param name="dev"></param>
    /// <param name="Address"></param>
    /// <param name="Host"></param>
    /// <param name="port"></param>
    /// <param name="value"></param>
    public void LED_SetAutoPowerList(int dev, byte Address, string Host, int port, ref AUTOPOWERTIME value)
    {
        DLL_LED_SetAutoPowerList(dev, Address, Host, port, ref value);
    }
    #endregion

    #region �����Ŀ���ݵ��ļ�
    //void (_stdcall *LED_SaveStreamToFile)(char *filename);
    [DllImport("LEDSender.DLL", EntryPoint = "LED_SaveStreamToFile")]
    public static extern void DLL_LED_SaveStreamToFile(string filename);

    /// <summary>
    /// ���γɵĽ�Ŀ���ݷ��͵���ʾ��
    /// </summary>
    /// <param name="filename"></param>
    public void LED_SaveStreamToFile(string filename)
    {
        DLL_LED_SaveStreamToFile(filename);
    }
    #endregion

    #region �������ݵ���ʾ��
    //void (_stdcall *LED_SendToScreen)(long dev, BYTE Address, char *Host, WORD port);
    [DllImport("LEDSender.DLL", EntryPoint = "LED_SendToScreen")]
    public static extern void DLL_LED_SendToScreen(int dev, byte Address, string Host, int port);

    /// <summary>
    /// ���γɵĽ�Ŀ���ݷ��͵���ʾ��
    /// </summary>
    /// <param name="dev"></param>
    /// <param name="Address"></param>
    /// <param name="Host"></param>
    /// <param name="port"></param>
    public void LED_SendToScreen(int dev, byte Address, string Host, int port)
    {
        DLL_LED_SendToScreen(dev, Address, Host, port);
    }
    #endregion

    #region �������ݵ���ʾ��
    //DWORD (_stdcall *LED_SendToScreenTest)(long dev, BYTE Address, char *Host, WORD port);
    [DllImport("LEDSender.DLL", EntryPoint = "LED_SendToScreenTest")]
    public static extern Int32 DLL_LED_SendToScreenTest(int dev, byte Address, string Host, int port);

    /// <summary>
    /// ���γɵĽ�Ŀ���ݷ��͵���ʾ��
    /// </summary>
    /// <param name="dev"></param>
    /// <param name="Address"></param>
    /// <param name="Host"></param>
    /// <param name="port"></param>
    public Int32 LED_SendToScreenTest(int dev, byte Address, string Host, int port)
    {
        return DLL_LED_SendToScreenTest(dev, Address, Host, port);
    }
    #endregion

    #region ��ȡ��ǰҳ��
    //void (_stdcall *LED_GetLeaf)(long dev, BYTE Address, char *Host, WORD port);
    [DllImport("LEDSender.DLL", EntryPoint = "LED_GetLeaf")]
    public static extern void DLL_LED_GetLeaf(int dev, byte Address, string Host, int port);
    /// <summary>
    /// ��ȡ��ǰҳ��
    /// </summary>
    /// <param name="dev"></param>
    /// <param name="Address"></param>
    /// <param name="Host"></param>
    /// <param name="port"></param>
    public void LED_GetLeaf(int dev, byte Address, string Host, int port)
    {
        DLL_LED_GetLeaf(dev, Address, Host, port);
    }
    #endregion

    #region ���õ�ǰҳ��
    //void (_stdcall *LED_SetLeaf)(long dev, BYTE Address, char *Host, WORD port, DWORD Value);
    [DllImport("LEDSender.DLL", EntryPoint = "LED_SetLeaf")]
    public static extern void DLL_LED_SetLeaf(int dev, byte Address, string Host, int port, uint Value);

    /// <summary>
    /// ���õ�ǰҳ��
    /// </summary>
    /// <param name="dev"></param>
    /// <param name="Address"></param>
    /// <param name="Host"></param>
    /// <param name="port"></param>
    /// <param name="Value">ҳ����</param>		
    public void LED_SetLeaf(int dev, byte Address, string Host, int port, uint Value)
    {
        DLL_LED_SetLeaf(dev, Address, Host, port, Value);
    }
    #endregion

    #region ��ȡ���ƿ���������
    //long (_stdcall *LED_GetLEDParamDirect)(long dev, BYTE Address, char *Host, long port, PBoardParam param);
    [DllImport("LEDSender.DLL", EntryPoint = "LED_GetLEDParamDirect")]
    public static extern int DLL_LED_GetLEDParamDirect(int dev, byte Address, string Host, int port, ref TBoardParam param);
    /// <summary>
    /// ��ȡ��Դ״̬
    /// </summary>
    /// <param name="dev"></param>
    /// <param name="Address"></param>
    /// <param name="Host"></param>
    /// <param name="port"></param>
    public int LED_GetLEDParamDirect(int dev, byte Address, string Host, int port, ref TBoardParam param)
    {
        return DLL_LED_GetLEDParamDirect(dev, Address, Host, port, ref param);
    }
    #endregion

    #region ������ƿ���������
    //void (_stdcall *LED_SetLEDParam)(long dev, BYTE Address, char *Host, long port, PBoardParam param);
    [DllImport("LEDSender.DLL", EntryPoint = "LED_SetLEDParam")]
    public static extern void DLL_LED_SetLEDParam(int dev, byte Address, string Host, int port, ref TBoardParam param);
    /// <summary>
    /// ��ȡ��Դ״̬
    /// </summary>
    /// <param name="dev"></param>
    /// <param name="Address"></param>
    /// <param name="Host"></param>
    /// <param name="port"></param>
    public void LED_SetLEDParam(int dev, byte Address, string Host, int port, ref TBoardParam param)
    {
        DLL_LED_SetLEDParam(dev, Address, Host, port, ref param);
    }
    #endregion

    #region ��ȡ��Դ״̬
    //void (_stdcall *LED_GetPower)(long dev, BYTE Address, char *Host, WORD port);
    [DllImport("LEDSender.DLL", EntryPoint = "LED_GetPower")]
    public static extern void DLL_LED_GetPower(int dev, byte Address, string Host, int port);
    /// <summary>
    /// ��ȡ��Դ״̬
    /// </summary>
    /// <param name="dev"></param>
    /// <param name="Address"></param>
    /// <param name="Host"></param>
    /// <param name="port"></param>
    public void LED_GetPower(int dev, byte Address, string Host, int port)
    {
        DLL_LED_GetPower(dev, Address, Host, port);
    }
    #endregion

    #region ������ʾ����Դ
    //void (_stdcall *LED_SetPower)(long dev, BYTE Address, char *Host, WORD port, DWORD Power);
    [DllImport("LEDSender.DLL", EntryPoint = "LED_SetPower")]
    public static extern void DLL_LED_SetPower(int dev, byte Address, string Host, int port, uint Power);

    /// <summary>
    /// �򿪻�ر���ʾ����Դ
    /// </summary>
    /// <param name="dev"></param>
    /// <param name="Address"></param>
    /// <param name="Host"></param>
    /// <param name="port"></param>
    /// <param name="Power">LED_POWER_ON �򿪵�Դ, LED_POWER_OFF �رյ�Դ</param>		
    public void LED_SetPower(int dev, byte Address, string Host, int port, ePowerStatus Power)
    {
        DLL_LED_SetPower(dev, Address, Host, port, (uint)Power);
    }
    #endregion

    #region ��ȡ��ʾ������
    //void (_stdcall *LED_GetBrightness)(long dev, BYTE Address, char *Host, WORD port);
    [DllImport("LEDSender.DLL", EntryPoint = "LED_GetBrightness")]
    public static extern void DLL_LED_GetBrightness(int dev, byte Address, string Host, int port);

    /// <summary>
    /// ��ȡ��ʾ������
    /// </summary>
    /// <param name="dev"></param>
    /// <param name="Address"></param>
    /// <param name="Host"></param>
    /// <param name="port"></param>
    public void LED_GetBrightness(int dev, byte Address, string Host, int port)
    {
        DLL_LED_GetBrightness(dev, Address, Host, port);
    }

    #endregion

    #region ������ʾ������
    //void (_stdcall *LED_SetBrightness)(long dev, BYTE Address, char *Host, WORD port, int Brightness);
    [DllImport("LEDSender.DLL", EntryPoint = "LED_SetBrightness")]
    public static extern void DLL_LED_SetBrightness(int dev, byte Address, string Host, int port, int Brightness);

    /// <summary>
    /// ������ʾ������
    /// </summary>
    /// <param name="dev"></param>
    /// <param name="Address"></param>
    /// <param name="Host"></param>
    /// <param name="port"></param>
    /// <param name="Brightness"></param>
    public void LED_SetBrightness(int dev, byte Address, string Host, int port, int Brightness)
    {
        DLL_LED_SetBrightness(dev, Address, Host, port, Brightness);
    }

    #endregion

    #region ��ȡ��Ϣ
    //void (_stdcall *LED_GetNotifyMessage)(PNotifyMessage Notify);
    [DllImport("LEDSender.DLL", EntryPoint = "LED_GetNotifyMessage")]
    public static extern void DLL_LED_GetNotifyMessage(ref NotifyMessage Notify);
    public void LED_GetNotifyMessage(ref NotifyMessage Notify)
    {
        DLL_LED_GetNotifyMessage(ref Notify);
    }
    #endregion

    #region ��ȡĳ���豸����Ϣ
    //long (_stdcall *LED_GetDeviceNotifyMessage)(long dev, PNotifyMessage Notify);
    [DllImport("LEDSender.DLL", EntryPoint = "LED_GetDeviceNotifyMessage")]
    public static extern int DLL_LED_GetDeviceNotifyMessage(int dev, ref NotifyMessage Notify);
    public int LED_GetDeviceNotifyMessage(int dev, ref NotifyMessage Notify)
    {
        return DLL_LED_GetDeviceNotifyMessage(dev, ref Notify);
    }
    #endregion

    #region ��ȡѡ��
    //long (_stdcall *LED_GetOption)(int Index)
    [DllImport("LEDSender.DLL", EntryPoint = "LED_GetOption")]
    public static extern int DLL_LED_GetOption(int Index);
    public void LED_GetOption(int Index)
    {
        DLL_LED_GetOption(Index);
    }
    #endregion

    #region ����ѡ��
    //long (_stdcall *LED_SetOption)(int Index, DWORD Value);
    [DllImport("LEDSender.DLL", EntryPoint = "LED_SetOption")]
    public static extern int DLL_LED_SetOption(int Index, uint Value);
    public void LED_SetOption(int Index, uint Value)
    {
        DLL_LED_SetOption(Index, Value);
    }
    #endregion

    #region ��ʼ�γ���ʾ������
    //long (_stdcall *MakeRoot)(long RootType, long ScreenType);
    [DllImport("LEDSender.DLL", EntryPoint = "MakeRoot")]
    public static extern int DLL_MakeRoot(int RootType, int ScreenType);

    /// <summary>
    /// ��ʼ�γ���ʾ������
    /// </summary>
    /// <param name="RootType">ROOT_PLAY �����ڲ���, ROOT_DOWNLOAD ���ز����ţ��������Ҫ���벻Ҫʹ������</param>
    /// <param name="ScreenType">��ʾ������ SCREEN_UNICOLOR ��ɫ��ʾ��, SCREEN_COLOR ˫ɫ��ʾ��</param>
    /// <returns></returns>
    public int MakeRoot(eRootType RootType, eScreenType ScreenType)
    {
        return DLL_MakeRoot((int)RootType, (int)ScreenType);
    }
    #endregion

    #region ��ʼ�γ���ʾ������
    //long (_stdcall *MakeRootEx)(long RootType, long ScreenType, long survive);
    [DllImport("LEDSender.DLL", EntryPoint = "MakeRootEx")]
    public static extern int DLL_MakeRootEx(int RootType, int ScreenType, int Survive);

    /// <summary>
    /// ��ʼ�γ���ʾ������
    /// </summary>
    /// <param name="RootType">ROOT_PLAY �����ڲ���, ROOT_DOWNLOAD ���ز����ţ��������Ҫ���벻Ҫʹ������</param>
    /// <param name="ScreenType">��ʾ������ SCREEN_UNICOLOR ��ɫ��ʾ��, SCREEN_COLOR ˫ɫ��ʾ��</param>
    /// <param name="Survive">��Ŀ��ʾ����Чʱ�䣬��ʱ��󣬻�ָ���ʾԭ���������صĽ�Ŀ</param>
    /// <returns></returns>
    public int MakeRootEx(eRootType RootType, eScreenType ScreenType, int Survive)
    {
        return DLL_MakeRootEx((int)RootType, (int)ScreenType, Survive);
    }
    #endregion

    #region ����ҳ�棬��ָ����ʾʱ��
    //long (_stdcall *AddLeaf)(long DisplayTime); 
    [DllImport("LEDSender.DLL", EntryPoint = "AddLeaf")]
    public static extern int DLL_AddLeaf(int DisplayTime);

    /// <summary>
    /// ����ҳ�棬��ָ����ʾʱ��
    /// </summary>
    /// <param name="DisplayTime">ҳ����ʾʱ�䣬��λΪ����(ms)</param>
    /// <returns></returns>
    public int AddLeaf(int DisplayTime)
    {
        return DLL_AddLeaf(DisplayTime);
    }
    #endregion

    #region �ڵ�ǰ��ʾҳ���ϴ���һ����ʾ������ʾ����������dc
    //long (_stdcall *AddWindow)(HDC dc,short width, short height, LPRECT rect, long method, long speed, long transparent);
    [DllImport("LEDSender.DLL", EntryPoint = "AddWindow")]
    public static extern int DLL_AddWindow(int dc, short width, short height, ref RECT rect, int method, int speed, int transparent);

    /// <summary>
    /// �ڵ�ǰ��ʾҳ���ϴ���һ����ʾ������ʾ����������dc
    /// </summary>
    /// <param name="dc">�豸���</param>
    /// <param name="width">��ȡ�Ŀ��</param>
    /// <param name="height">��ȡ�ĸ߶�</param>
    /// <param name="rect">��ʾ����</param>
    /// <param name="method">��ʾ��ʽ</param>
    /// <param name="speed">��ʾ�ٶ�</param>
    /// <param name="transparent">�Ƿ�͸��</param>
    /// <returns></returns>
    public int AddWindow(int dc, short width, short height, ref RECT rect, int method, int speed, int transparent)
    {
        return DLL_AddWindow(dc, width, height, ref rect, method, speed, transparent);
    }
    #endregion

    #region �ڵ�ǰ��ʾҳ���ϴ���һ����ʾ������ʾ����������ͼƬ�ļ�
    //long (_stdcall *AddPicture)(char *filename, LPRECT rect, long method, long speed, long transparent, long stretch);
    [DllImport("LEDSender.DLL", EntryPoint = "AddPicture")]
    public static extern int DLL_AddPicture(string filename, ref RECT rect, int method, int speed, int transparent, int stretch);

    /// <summary>
    /// �ڵ�ǰ��ʾҳ���ϴ���һ����ʾ������ʾ����������dc
    /// </summary>
    /// <param name="filemane">ͼƬ�ļ���</param>
    /// <param name="rect">��ʾ����</param>
    /// <param name="method">��ʾ��ʽ</param>
    /// <param name="speed">��ʾ�ٶ�</param>
    /// <param name="transparent">�Ƿ�͸��</param>
    /// <param name="stretch">�Ƿ�����ʾ��������</param>
    /// <returns></returns>
    public int AddPicture(string filename, ref RECT rect, int method, int speed, int transparent, int stretch)
    {
        return DLL_AddPicture(filename, ref rect, method, speed, transparent, stretch);
    }
    #endregion

    #region  �ڵ�ǰҳ�洴��һ������ʱ��
    //long (_stdcall *AddDateTime)(LPRECT rect, long transparent, char *fontname, long fontsize, long fontcolor, long format, long fontstyle);
    [DllImport("LEDSender.DLL", EntryPoint = "AddDateTime")]
    public static extern int DLL_AddDateTime(ref RECT rect, int transparent, string fontname, int fontsize, int fontcolor, int format, int fontstyle);
    /// <summary>
    /// �ڵ�ǰҳ�洴��һ������ʱ��
    /// </summary>
    /// <param name="rect">��ʾ����</param>
    /// <param name="transparent">�Ƿ�͸��</param>
    /// <param name="fontname">������</param>
    /// <param name="fontsize">�����С</param>
    /// <param name="fontcolor">������ɫ</param>
    /// <param name="format">ʱ�Ӹ�ʽ</param>
    /// <returns></returns>
    public int AddDateTime(ref RECT rect, int transparent, string fontname, int fontsize, int fontcolor, eTimeFormat format, int fontstyle)
    {
        return DLL_AddDateTime(ref rect, transparent, fontname, fontsize, fontcolor, (int)format, fontstyle);
    }
    #endregion

    #region  ��ʾ���뺺��
    //long (_stdcall *AddString)(char *str, LPRECT rect, long method, long speed, long transparent, long fontset, long fontcolor);
    [DllImport("LEDSender.DLL", EntryPoint = "AddString")]
    public static extern int DLL_AddString(string str, ref RECT rect, int method, int speed, int transparent, int fontset, int fontcolor);

    public int AddString(string str, ref RECT rect, int method, int speed, int transparent, int fontset, int fontcolor)
    {
        return DLL_AddString(str, ref rect, method, speed, transparent, fontset, fontcolor);
    }
    #endregion

    #region ��ʾwindows����
    //long (_stdcall *AddText)(char *str, LPRECT rect, long method, long speed, long transparent, char *fontname, long fontsize, long fontcolor, long fontstyle = 0);
    [DllImport("LEDSender.DLL", EntryPoint = "AddText")]
    public static extern int DLL_AddText(string str, ref RECT rect, int method, int speed, int transparent, string fontname, int fontsize, int fontcolor, int fontstyle);

    public int AddText(string str, ref RECT rect, int method, int speed, int transparent, string fontname, int fontsize, int fontcolor, int fontstyle)
    {
        return DLL_AddText(str, ref rect, method, speed, transparent, fontname, fontsize, fontcolor, fontstyle);
    }
    #endregion

    #region ��ʾwindows���֣���������
    //long (_stdcall *AddTextEx)(char *str, LPRECT rect, long method, long speed, long transparent, char *fontname, long fontsize, long fontcolor, long fontstyle = 0, long wordwrap = 0);
    [DllImport("LEDSender.DLL", EntryPoint = "AddTextEx")]
    public static extern int DLL_AddTextEx(string str, ref RECT rect, int method, int speed, int transparent, string fontname, int fontsize, int fontcolor, int fontstyle, int wordwrap);

    public int AddTextEx(string str, ref RECT rect, int method, int speed, int transparent, string fontname, int fontsize, int fontcolor, int fontstyle, int wordwrap)
    {
        return DLL_AddTextEx(str, ref rect, method, speed, transparent, fontname, fontsize, fontcolor, fontstyle, wordwrap);
    }
    #endregion

    #region ��ʾ����
    //long (_stdcall *AddMovie)(char *filename, LPRECT rect, long stretch);
    [DllImport("LEDSender.DLL", EntryPoint = "AddMovie")]
    public static extern int DLL_AddMovie(string filename, ref RECT rect, int stretch);

    public int AddMovie(string filename, ref RECT rect, int stretch)
    {
        return DLL_AddMovie(filename, ref rect, stretch);
    }
    #endregion

    #region ��ǰҳ�洴��һ������ʱ��ʾ����
    //long (_stdcall *AddCountUp)(LPRECT rect, long transparent, char *fontname, long fontsize, long fontcolor, long format, LPSYSTEMTIME starttime, long fontstyle);
    [DllImport("LEDSender.DLL", EntryPoint = "AddCountUp")]
    public static extern int DLL_AddCountUp(ref RECT rect, int transparent, string fontname, int fontsize, int fontcolor, int format, ref SYSTEMTIME starttime, int fontstyle);
    public int AddCountUp(ref RECT rect, int transparent, string fontname, int fontsize, int fontcolor, eCountType format, ref SYSTEMTIME starttime, int fontstyle)
    {
        return DLL_AddCountUp(ref rect, transparent, fontname, fontsize, fontcolor, (int)format, ref starttime, fontstyle);
    }
    #endregion

    #region �ڵ�ǰ��ʾҳ�洴��һ������ʱ��ʾ����
    //long (_stdcall *AddCountDown)(LPRECT rect,long transparent, char *fontname, long fontsize, long fontcolor, long format, LPSYSTEMTIME endtime, long fontstyle);
    [DllImport("LEDSender.DLL", EntryPoint = "AddCountDown")]
    public static extern int DLL_AddCountDown(ref RECT rect, int transparent, string fontname, int fontsize, int fontcolor, int format, ref SYSTEMTIME endtime, int fontstyle);
    public int AddCountDown(ref RECT rect, int transparent, string fontname, int fontsize, int fontcolor, eCountType format, ref SYSTEMTIME endtime, int fontstyle)
    {
        return DLL_AddCountDown(ref rect, transparent, fontname, fontsize, fontcolor, (int)format, ref endtime, fontstyle);
    }
    #endregion

    #region �ڵ�ǰ��ʾҳ�洴��һ���¶���ʾ����
    //Function AddTemperature(ARect: PRect; Transparent: Integer; FontName: PChar; FontSize: Integer; FontColor: Integer; FontStyle: Integer = 0): Integer; Stdcall;
    [DllImport("LEDSender.DLL", EntryPoint = "AddTemperature")]
    public static extern int DLL_AddTemperature(ref RECT rect, int transparent, string fontname, int fontsize, int fontcolor, int fontstyle);
    public int AddTemperature(ref RECT rect, int transparent, string fontname, int fontsize, int fontcolor, int fontstyle)
    {
        return DLL_AddTemperature(ref rect, transparent, fontname, fontsize, fontcolor, fontstyle);
    }
    #endregion

    #region �ڵ�ǰ��ʾҳ�洴��һ��ʪ����ʾ����
    //Function AddHumidity(ARect: PRect; Transparent: Integer; FontName: PChar; FontSize: Integer; FontColor: Integer; FontStyle: Integer = 0): Integer; Stdcall;
    [DllImport("LEDSender.DLL", EntryPoint = "AddHumidity")]
    public static extern int DLL_AddHumidity(ref RECT rect, int transparent, string fontname, int fontsize, int fontcolor, int fontstyle);
    public int AddHumidity(ref RECT rect, int transparent, string fontname, int fontsize, int fontcolor, int fontstyle)
    {
        return DLL_AddHumidity(ref rect, transparent, fontname, fontsize, fontcolor, fontstyle);
    }
    #endregion

    #region �ڵ�ǰ��ʾҳ�洴��һ��ģ��ʱ��
    //long (_stdcall *AddClock)(LPRECT rect, long transparent, long WidthH, long WidthM, long DotH, long DotM, DWORD ColorH, DWORD ColorM, DWORD ColorS, DWORD ColorD, DWORD ColorN);
    [DllImport("LEDSender.DLL", EntryPoint = "AddClock")]
    public static extern int DLL_AddClock(ref RECT rect, int transparent, int WidthH, int WidthM, int DotH, int DotM, uint ColorH, uint ColorM, uint ColorS, uint ColorD, uint ColorN);
    /// <summary>
    /// �ڵ�ǰ��ʾҳ���ϴ���һ����ʾ������ʾ����������dc
    /// </summary>
    /// <param name="rect">��ʾ����</param>
    /// <param name="transparent">�Ƿ�͸��</param>
    /// <param name="DotM">����뾶</param>
    /// <param name="DotH">3,6,9��뾶</param>
    /// <param name="ColorH">Сʱָ����ɫ</param>
    /// <param name="ColorM">����ָ����ɫ</param>
    /// <param name="ColorS">��ָ����ɫ</param>
    /// <param name="ColorD">3,6,9����ɫ</param>
    /// <param name="ColorN">������ɫ</param>
    /// <param name="WidthH">Сʱָ����</param>
    /// <param name="WidthM">����ָ����</param>
    /// <returns></returns>
    public int AddClock(ref RECT rect, int transparent, int WidthH, int WidthM, int DotH, int DotM, uint ColorH, uint ColorM, uint ColorS, uint ColorD, uint ColorN)
    {
        return DLL_AddClock(ref rect, transparent, WidthH, WidthM, DotH, DotM, ColorH, ColorM, ColorS, ColorD, ColorN);
    }
    #endregion

    #region ��ȡ��ʼ�������ݣ���16���Ʊ��浽ָ���ı��ļ���
    //long (_stdcall *LED_ExportBeginPacket)(char *filename, long address);
    [DllImport("LEDSender.DLL", EntryPoint = "LED_ExportBeginPacket")]
    public static extern int DLL_LED_ExportBeginPacket(string filename, int address);

    public int LED_ExportBeginPacket(string filename, int address)
    {
        return DLL_LED_ExportBeginPacket(filename, address);
    }
    #endregion

    #region ��ȡ���ݰ�������
    //long (_stdcall *LED_GetDataPacketCount)();
    [DllImport("LEDSender.DLL", EntryPoint = "LED_GetDataPacketCount")]
    public static extern int DLL_LED_GetDataPacketCount();

    public int LED_GetDataPacketCount()
    {
        return DLL_LED_GetDataPacketCount();
    }
    #endregion

    #region ��ȡ���ݰ������ݣ���16���Ʊ��浽ָ���ı��ļ���
    //long (_stdcall *LED_ExportDataPacket)(char *filename, long address, long serialno);
    [DllImport("LEDSender.DLL", EntryPoint = "LED_ExportDataPacket")]
    public static extern int DLL_LED_ExportDataPacket(string filename, int address, int serialno);

    public int LED_ExportDataPacket(string filename, int address, int serialno)
    {
        return DLL_LED_ExportDataPacket(filename, address, serialno);
    }
    #endregion

    #region ��ȡ�����������ݣ���16���Ʊ��浽ָ���ı��ļ���
    //long (_stdcall *LED_ExportEndPacket)(char *filename, long address, long serialno);
    [DllImport("LEDSender.DLL", EntryPoint = "LED_ExportEndPacket")]
    public static extern int DLL_LED_ExportEndPacket(string filename, int address, int serialno);

    public int LED_ExportEndPacket(string filename, int address, int serialno)
    {
        return DLL_LED_ExportEndPacket(filename, address, serialno);
    }
    #endregion

    #region ��ȡ���õ�Դ״̬�����ݰ�����16���Ʊ��浽ָ���ı��ļ���
    //long (_stdcall *LED_ExportSetPowerPacket)(char *filename, long address, long value);
    [DllImport("LEDSender.DLL", EntryPoint = "LED_ExportSetPowerPacket")]
    public static extern int DLL_LED_ExportSetPowerPacket(string filename, int address, int value);

    public int LED_ExportSetPowerPacket(string filename, int address, int value)
    {
        return DLL_LED_ExportSetPowerPacket(filename, address, value);
    }
    #endregion

    #region ��ȡ�������ȵ����ݰ�����16���Ʊ��浽ָ���ı��ļ���
    //long (_stdcall *LED_ExportSetBrightPacket)(char *filename, long address, long value);
    [DllImport("LEDSender.DLL", EntryPoint = "LED_ExportSetBrightPacket")]
    public static extern int DLL_LED_ExportSetBrightPacket(string filename, int address, int value);

    public int LED_ExportSetBrightPacket(string filename, int address, int value)
    {
        return DLL_LED_ExportSetBrightPacket(filename, address, value);
    }
    #endregion

    #region ��ʼ����ͼ����
    //function UserCanvas_Init(AWidth: Integer; AHeight: Integer): Integer; stdcall;
    [DllImport("LEDSender.DLL", EntryPoint = "UserCanvas_Init")]
    public static extern int DLL_UserCanvas_Init(int width, int height);

    public int UserCanvas_Init(int width, int height)
    {
        return DLL_UserCanvas_Init(width, height);
    }
    #endregion

    #region ����ֱ��
    //function UserCanvas_Draw_Line(X, Y, X1, Y1: Integer; ALineWidth: Integer; AColor: Integer): Integer; stdcall;
    [DllImport("LEDSender.DLL", EntryPoint = "UserCanvas_Draw_Line")]
    public static extern int DLL_UserCanvas_Draw_Line(int x, int y, int x1, int y1, int linewidth, int color);

    public int UserCanvas_Draw_Line(int x, int y, int x1, int y1, int linewidth, int color)
    {
        return DLL_UserCanvas_Draw_Line(x, y, x1, y1, linewidth, color);
    }
    #endregion

    #region ���ƾ���
    //function UserCanvas_Draw_Rectangle(X, Y, X1, Y1: Integer; ALineWidth: Integer; AColor: Integer; AFillColor: Integer): Integer; stdcall;
    [DllImport("LEDSender.DLL", EntryPoint = "UserCanvas_Draw_Rectangle")]
    public static extern int DLL_UserCanvas_Draw_Rectangle(int x, int y, int x1, int y1, int linewidth, int color, int fillcolor);

    public int UserCanvas_Draw_Rectangle(int x, int y, int x1, int y1, int linewidth, int color, int fillcolor)
    {
        return DLL_UserCanvas_Draw_Rectangle(x, y, x1, y1, linewidth, color, fillcolor);
    }
    #endregion


    #region ������Բ��Բ��
    //function UserCanvas_Draw_Ellipse(X, Y, X1, Y1: Integer; ALineWidth: Integer; AColor: Integer; AFillColor: Integer): Integer; stdcall;
    [DllImport("LEDSender.DLL", EntryPoint = "UserCanvas_Draw_Ellipse")]
    public static extern int DLL_UserCanvas_Draw_Ellipse(int x, int y, int x1, int y1, int linewidth, int color, int fillcolor);

    public int UserCanvas_Draw_Ellipse(int x, int y, int x1, int y1, int linewidth, int color, int fillcolor)
    {
        return DLL_UserCanvas_Draw_Ellipse(x, y, x1, y1, linewidth, color, fillcolor);
    }
    #endregion

    #region ����Բ�Ǿ���
    //function UserCanvas_Draw_RoundRect(X, Y, X1, Y1: Integer; ALineWidth: Integer; AColor: Integer; AFillColor: Integer; ARoundX, ARoundY: Integer): Integer; stdcall;
    [DllImport("LEDSender.DLL", EntryPoint = "UserCanvas_Draw_RoundRect")]
    public static extern int DLL_UserCanvas_Draw_RoundRect(int x, int y, int x1, int y1, int linewidth, int color, int fillcolor, int roundx, int roundy);

    public int UserCanvas_Draw_RoundRect(int x, int y, int x1, int y1, int linewidth, int color, int fillcolor, int roundx, int roundy)
    {
        return DLL_UserCanvas_Draw_RoundRect(x, y, x1, y1, linewidth, color, fillcolor, roundx, roundy);
    }
    #endregion

    #region ��������
    //function UserCanvas_Draw_Text(X, Y, CX, CY: Integer; Str: PChar; FontName: PChar; FontSize, FontColor, FontStyle: Integer; Alignment: Integer): Integer; stdcall;
    [DllImport("LEDSender.DLL", EntryPoint = "UserCanvas_Draw_Text")]
    public static extern int DLL_UserCanvas_Draw_Text(int x, int y, int x1, int y1, string str, string fontname, int fontsize, int fontcolor, int fontstyle, int alignment);

    public int UserCanvas_Draw_Text(int x, int y, int x1, int y1, string str, string fontname, int fontsize, int fontcolor, int fontstyle, int alignment)
    {
        return DLL_UserCanvas_Draw_Text(x, y, x1, y1, str, fontname, fontsize, fontcolor, fontstyle, alignment);
    }
    #endregion

    #region �����Ƶ�ͼƬ��ӵ���ʾ��������
    //function AddUserCanvas(ARect: PRect; Method, Speed, Transparent: Integer): Integer; stdcall;
    [DllImport("LEDSender.DLL", EntryPoint = "AddUserCanvas")]
    public static extern int DLL_AddUserCanvas(ref RECT rect, int method, int speed, int transparent);

    public int AddUserCanvas(ref RECT rect, int method, int speed, int transparent)
    {
        return DLL_AddUserCanvas(ref rect, method, speed, transparent);
    }
    #endregion

}


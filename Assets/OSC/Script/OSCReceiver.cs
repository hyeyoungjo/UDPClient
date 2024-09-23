using System.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityOSC;
using TMPro;

public class OSCReceiver : MonoBehaviour, OSCListener
{
    OSCServer myServer;
    public int inPort = 9998;
    //private int buffersize = 100;
    DxlController dxlValue;
    public TextMeshProUGUI txt;
    private string text_received;
    private string text_received_prev;
    
    void Start()
    {

        OSCHandler.Instance.Init();
        myServer = OSCHandler.Instance.CreateServer("myServer", inPort);
        OSCHandler.Instance.AddCallback(this);

        myServer.ReceiveBufferSize = 1024;
        myServer.SleepMilliseconds = 10;
        dxlValue = GetComponent<DxlController>();

        text_received = "test string";
        text_received_prev = text_received;

    }

    public void OnOSC(OSCPacket pckt)
    {


        if (pckt.Address.Equals("/position_feedback"))
        {
            string receivedData = pckt.Data[0].ToString();
            dxlValue.positionFeedback = int.Parse(receivedData);
        }
        else if(pckt.Address.Equals("/load_feedback"))
        {
            string receivedData = pckt.Data[0].ToString();
            dxlValue.loadFeedback = int.Parse(receivedData);
        }
        else if (pckt.Address.Equals("/load_feedback"))
        {
            string receivedData = pckt.Data[0].ToString();
            dxlValue.tempFeedback = int.Parse(receivedData);
        }
        // custom tag here
        else if (pckt.Address.Equals("/text_input"))
        {
            text_received = pckt.Data[0].ToString();
        }

    }
    void Update() 
    {
        SetText();
    }

    
    void SetText()
    {
        if(text_received!=null)
        {
            if(text_received != text_received_prev)
            {
                print("text received: " + text_received);
                txt.text = text_received;
                text_received_prev = text_received;
            }
        }
    }


}

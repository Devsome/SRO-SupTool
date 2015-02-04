using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace VEGA.Framework.Proxy
{
    class Main
    {
        public delegate void LogMsg(string message);
        public static event LogMsg OnLogMsg;

        #region "Variables"
        private TcpListener gw_local_server;
        private TcpClient gw_local_client;
        private TcpClient gw_remote_client;
        private Security gw_local_security;
        private Security gw_remote_security;
        private NetworkStream gw_local_stream;
        private NetworkStream gw_remote_stream;
        private TransferBuffer gw_remote_recv_buffer;
        private List<Packet> gw_remote_recv_packets;
        private List<KeyValuePair<TransferBuffer, Packet>> gw_remote_send_buffers;
        private TransferBuffer gw_local_recv_buffer;
        private List<Packet> gw_local_recv_packets;
        private List<KeyValuePair<TransferBuffer, Packet>> gw_local_send_buffers;

        private TcpListener ag_local_server;
        private TcpClient ag_local_client;
        private TcpClient ag_remote_client;
        private Security ag_local_security;
        public static Security ag_remote_security;
        private NetworkStream ag_local_stream;
        private NetworkStream ag_remote_stream;
        private TransferBuffer ag_remote_recv_buffer;
        private List<Packet> ag_remote_recv_packets;
        private List<KeyValuePair<TransferBuffer, Packet>> ag_remote_send_buffers;
        private TransferBuffer ag_local_recv_buffer;
        private List<Packet> ag_local_recv_packets;
        private List<KeyValuePair<TransferBuffer, Packet>> ag_local_send_buffers;

        private string xfer_remote_ip;
        private int xfer_remote_port;

        private object exit_lock = new object();
        private bool should_exit = false;
        #endregion
        #region "Threads"
        private Thread thProxy;
        private Thread thGateway;
        private Thread thAgent;
        #endregion

        public void Start()
        {
            try
            {
                thProxy = new Thread(StartThread);
                thProxy.Start();
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        private void StartThread()
        {
            try
            {
                thGateway = new Thread(GatewayThread);
                thGateway.Start();

                thAgent = new Thread(AgentThread);
                thAgent.Start();

                thGateway.Join();
                thAgent.Join();
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public void Stop()
        {
            if (thProxy == null)
                return;
            else
            {
                gw_local_server.Stop();
                ag_local_server.Stop();
                thGateway.Abort();
                thAgent.Abort();
            }
        }

        private void GatewayRemoteThread()
        {
            try
            {
                while (true)
                {
                    lock (exit_lock)
                    {
                        if (should_exit)
                            break;
                    }

                    if (gw_remote_stream.DataAvailable)
                    {
                        gw_remote_recv_buffer.Offset = 0;
                        gw_remote_recv_buffer.Size = gw_remote_stream.Read(gw_remote_recv_buffer.Buffer, 0, gw_remote_recv_buffer.Buffer.Length);
                        gw_remote_security.Recv(gw_remote_recv_buffer);
                    }

                    gw_remote_recv_packets = gw_remote_security.TransferIncoming();
                    if (gw_remote_recv_packets != null)
                    {
                        foreach (Packet packet in gw_remote_recv_packets)
                        {
                            byte[] packet_bytes = packet.GetBytes();

                            if (packet.Opcode == 0x5000 || packet.Opcode == 0x9000)
                            {
                                continue;
                            }

                            if (packet.Opcode == 0xA102)
                            {
                                byte result = packet.ReadUInt8();
                                if (result == 1)
                                {
                                    uint id = packet.ReadUInt32();
                                    string ip = packet.ReadAscii();
                                    ushort port = packet.ReadUInt16();

                                    xfer_remote_ip = ip;
                                    xfer_remote_port = port;

                                    Packet new_packet = new Packet(0xA102, true);
                                    new_packet.WriteUInt8(result);
                                    new_packet.WriteUInt32(id);
                                    new_packet.WriteAscii(Global.ProxyGlobal.Proxy_IPAddress);
                                    new_packet.WriteUInt16(Global.ProxyGlobal.Proxy_Agent_Port);

                                    gw_local_security.Send(new_packet);

                                    continue;
                                }
                            }

                            gw_local_security.Send(packet);
                        }
                    }

                    gw_remote_send_buffers = gw_remote_security.TransferOutgoing();
                    if (gw_remote_send_buffers != null)
                    {
                        foreach (var kvp in gw_remote_send_buffers)
                        {
                            Packet packet = kvp.Value;
                            TransferBuffer buffer = kvp.Key;

                            byte[] packet_bytes = packet.GetBytes();

                            gw_remote_stream.Write(buffer.Buffer, 0, buffer.Size);
                        }
                    }

                    Thread.Sleep(1);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("[GatewayRemoteThread] Exception: {0}", ex);
            }
        }
        private void GatewayLocalThread()
        {
            try
            {
                while (true)
                {
                    lock (exit_lock)
                    {
                        if (should_exit)
                            break;
                    }

                    if (gw_local_stream.DataAvailable)
                    {
                        gw_local_recv_buffer.Offset = 0;
                        gw_local_recv_buffer.Size = gw_local_stream.Read(gw_local_recv_buffer.Buffer, 0, gw_local_recv_buffer.Buffer.Length);
                        gw_local_security.Recv(gw_local_recv_buffer);
                    }

                    gw_local_recv_packets = gw_local_security.TransferIncoming();
                    if (gw_local_recv_packets != null)
                    {
                        foreach (Packet packet in gw_local_recv_packets)
                        {
                            if (packet.Opcode == 0x5000 || packet.Opcode == 0x9000 || packet.Opcode == 0x2001)
                                continue;

                            //////////////////////////////////////////////////////////////////////////
                            //SEND PACKET TO GW
                            //////////////////////////////////////////////////////////////////////////

                            //Send -> Packet to GW
                            gw_remote_security.Send(packet);

                            //Process-> Packet
                            byte[] packet_bytes = packet.GetBytes();
                            Framework.Bot.Packet.Handling.handlePacket(packet.Opcode, packet_bytes);

                            //Log -> Packet
                            BotLog(2, packet, packet_bytes);
                        }
                    }

                    gw_local_send_buffers = gw_local_security.TransferOutgoing();

                    if (gw_local_send_buffers != null)
                    {
                        foreach (var kvp in gw_local_send_buffers)
                        {
                            Packet packet = kvp.Value;
                            TransferBuffer buffer = kvp.Key;

                            //////////////////////////////////////////////////////////////////////////
                            //RECEIVED PACKET FROM GW
                            //////////////////////////////////////////////////////////////////////////

                            //Send -> Packet to Client
                            gw_local_stream.Write(buffer.Buffer, 0, buffer.Size);

                            //Process -> Packet
                            Framework.Bot.Packet.Handling.handlePacket(buffer.Buffer);

                            //Log -> Packet
                            BotLog(0, packet, buffer.Buffer);
                        }
                    }

                    Thread.Sleep(1);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("[GatewayLocalThread] Exception: {0}", ex);
            }
        }
        private void GatewayThread()
        {
            try
            {
                gw_local_security = new Security();
                gw_local_security.GenerateSecurity(true, true, true);
                gw_remote_security = new Security();
                gw_remote_recv_buffer = new TransferBuffer(4096, 0, 0);
                gw_local_recv_buffer = new TransferBuffer(4096, 0, 0);
                gw_local_server = new TcpListener(IPAddress.Parse(Global.ProxyGlobal.Proxy_IPAddress), Global.ProxyGlobal.Proxy_Gateway_Port);
                gw_local_server.Start();
                gw_local_client = gw_local_server.AcceptTcpClient();
                gw_remote_client = new TcpClient();
                gw_local_server.Stop();

                gw_remote_client.Connect(Global.ProxyGlobal.Server_Address, Global.ProxyGlobal.Server_Gateway_Port);

                gw_local_stream = gw_local_client.GetStream();
                gw_remote_stream = gw_remote_client.GetStream();

                Thread remote_thread = new Thread(GatewayRemoteThread);
                remote_thread.Start();

                Thread local_thread = new Thread(GatewayLocalThread);
                local_thread.Start();

                remote_thread.Join();
                local_thread.Join();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[GatewayThread] Exception: {0}", ex);
            }
        }

        private void AgentRemoteThread()
        {
            try
            {
                while (true)
                {
                    lock (exit_lock)
                    {
                        if (should_exit)
                            break;
                    }

                    if (ag_remote_stream.DataAvailable)
                    {
                        ag_remote_recv_buffer.Offset = 0;
                        ag_remote_recv_buffer.Size = ag_remote_stream.Read(ag_remote_recv_buffer.Buffer, 0, ag_remote_recv_buffer.Buffer.Length);
                        ag_remote_security.Recv(ag_remote_recv_buffer);
                    }

                    ag_remote_recv_packets = ag_remote_security.TransferIncoming();
                    if (ag_remote_recv_packets != null)
                    {
                        foreach (Packet packet in ag_remote_recv_packets)
                        {
                            byte[] packet_bytes = packet.GetBytes();

                            if (packet.Opcode == 0x5000 || packet.Opcode == 0x9000)
                                continue;

                            ag_local_security.Send(packet);
                        }
                    }

                    ag_remote_send_buffers = ag_remote_security.TransferOutgoing();
                    if (ag_remote_send_buffers != null)
                    {
                        foreach (var kvp in ag_remote_send_buffers)
                        {
                            Packet packet = kvp.Value;
                            TransferBuffer buffer = kvp.Key;
                            ag_remote_stream.Write(buffer.Buffer, 0, buffer.Size);
                        }
                    }

                    Thread.Sleep(1);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("[AgentRemoteThread] Exception: {0}", ex);
            }
        }
        private void AgentLocalThread()
        {
            try
            {
                while (true)
                {
                    lock (exit_lock)
                    {
                        if (should_exit)
                            break;
                    }

                    if (ag_local_stream.DataAvailable)
                    {
                        ag_local_recv_buffer.Offset = 0;
                        ag_local_recv_buffer.Size = ag_local_stream.Read(ag_local_recv_buffer.Buffer, 0, ag_local_recv_buffer.Buffer.Length);
                        ag_local_security.Recv(ag_local_recv_buffer);
                    }

                    ag_local_recv_packets = ag_local_security.TransferIncoming();
                    if (ag_local_recv_packets != null)
                    {
                        foreach (Packet packet in ag_local_recv_packets)
                        {
                            if (packet.Opcode == 0x5000 || packet.Opcode == 0x9000 || packet.Opcode == 0x2001)
                                continue;

                            //Send -> Packet to AG
                            ag_remote_security.Send(packet);

                            //Process -> Packet
                            byte[] packet_bytes = packet.GetBytes();
                            Framework.Bot.Packet.Handling.handlePacket(packet.Opcode, packet_bytes);

                            //Log -> Packet
                            BotLog(3, packet, packet_bytes);
                        }
                    }

                    ag_local_send_buffers = ag_local_security.TransferOutgoing();
                    if (ag_local_send_buffers != null)
                    {
                        foreach (var kvp in ag_local_send_buffers)
                        {
                            Packet packet = kvp.Value;
                            TransferBuffer buffer = kvp.Key;

                            //////////////////////////////////////////////////////////////////////////
                            //RECEIVED PACKET FROM AG
                            //////////////////////////////////////////////////////////////////////////

                            //Send -> Packet to Client
                            ag_local_stream.Write(buffer.Buffer, 0, buffer.Size);

                            //Process -> Packet
                            Framework.Bot.Packet.Handling.handlePacket(buffer.Buffer);

                            //Log -> Packet
                            BotLog(1, packet, buffer.Buffer);
                        }
                    }

                    Thread.Sleep(1);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("[AgentLocalThread] Exception: {0}", ex);
            }
        }
        private void AgentThread()
        {
            try
            {
                ag_local_security = new Security();
                ag_local_security.GenerateSecurity(true, true, true);

                ag_remote_security = new Security();

                ag_remote_recv_buffer = new TransferBuffer(4096, 0, 0);
                ag_local_recv_buffer = new TransferBuffer(4096, 0, 0);

                ag_local_server = new TcpListener(IPAddress.Parse(Global.ProxyGlobal.Proxy_IPAddress), Global.ProxyGlobal.Proxy_Agent_Port);
                ag_local_server.Start();

                Console.WriteLine("* Waiting for a connection... ");
                OnLogMsg("* Waiting for a connection... ");

                ag_local_client = ag_local_server.AcceptTcpClient();
                ag_remote_client = new TcpClient();

                Console.WriteLine("A connection has been made!");
                OnLogMsg("* A connection has been made!");

                ag_local_server.Stop();

                Console.WriteLine("Connecting to {0}:{1}", xfer_remote_ip, xfer_remote_port);

                ag_remote_client.Connect(xfer_remote_ip, xfer_remote_port);

                Console.WriteLine("The connection has been made!");
                OnLogMsg("* The connection has been made!");

                
                ag_local_stream = ag_local_client.GetStream();
                ag_remote_stream = ag_remote_client.GetStream();

                Thread remote_thread = new Thread(AgentRemoteThread);
                remote_thread.Start();

                Thread local_thread = new Thread(AgentLocalThread);
                local_thread.Start();

                remote_thread.Join();
                local_thread.Join();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[AgentThread] Exception: {0}", ex);
            }
        }

        private void BotLog(int id, Packet packet, byte[] buffer)
        {
            string datalen;
            string opc;
            string data;

            switch (id)
            {
                case 0:
                    //Console.Write("[GW|RECV] ");
                    break;
                case 1:
                    //Console.Write("[AG|RECV] ");
                    break;
                case 2:
                    //Console.Write("[GW|SEND] ");
                    break;
                case 3:
                    //Console.Write("[AG|SEND] ");
                    break;
            }

            datalen = buffer.Length.ToString();
            opc = string.Format("{0:X4}", packet.Opcode);
            data = BitConverter.ToString(buffer).Replace("-", "");

            string logMessage = string.Format("[DATALEN:{0}] [OPC:{1}] [DATA:{2}]", datalen, opc, data);
            //Console.WriteLine(logMessage);
        }
    }
}

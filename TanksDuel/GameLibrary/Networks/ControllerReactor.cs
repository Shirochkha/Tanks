using GameEngine.Input;
using Newtonsoft.Json;
using System;
using System.Net.Sockets;
using System.Text;

namespace GameLibrary.Networks
{
    /// <summary>
    /// Класс для передачи событий по сети
    /// </summary>
    public class ControllerReactor
    {
        private readonly Socket _other;

        /// <summary>
        /// Контроллер
        /// </summary>
        public IController Controller { get; }

        /// <summary>
        /// Конструктор класса для передачи событий по сети
        /// </summary>
        public ControllerReactor(IController controller, Socket other)
        {
            Controller = controller;
            _other = other;
            Controller.StartMoving += _controller_StartMoving;
            Controller.StopMoving += _controller_StopMoving;
            Controller.Shooting += _controller_Shooting;
        }

        private void _controller_Shooting(object sender, EventArgs e)
        {
            var msg = Message.EmptyWithType(MessageType.Shooting);

            using (NetworkStream networkStream = new NetworkStream(_other))
            {
                var messageBytes = Encoding.UTF8.GetBytes(msg.ToString() + "*");
                networkStream.Write(messageBytes, 0, messageBytes.Length);
            }
        }

        private void _controller_StopMoving(object sender, EventArgs e)
        {
            var msg = Message.EmptyWithType(MessageType.StopMoving);

            using (NetworkStream networkStream = new NetworkStream(_other))
            {
                var messageBytes = Encoding.UTF8.GetBytes(msg.ToString() + "*");
                networkStream.Write(messageBytes, 0, messageBytes.Length);
            }
        }

        private void _controller_StartMoving(object sender, EventArgs e)
        {
            var location = Controller.Tank.Location;
            var direction = Controller.CurrentDirection;

            var msg = new Message(MessageType.StartMoving, $"{location};{direction}");

            using (NetworkStream networkStream = new NetworkStream(_other))
            {
                var messageBytes = Encoding.UTF8.GetBytes(msg.ToString() + "*");
                networkStream.Write(messageBytes, 0, messageBytes.Length);
            }
        }
    }
}
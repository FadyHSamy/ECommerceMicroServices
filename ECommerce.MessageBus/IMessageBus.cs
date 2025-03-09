﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.MessageBus
{
    public interface IMessageBus
    {
        Task PublishMessage(object message,string topicQueueName);
    }
}

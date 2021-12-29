using MyRemote.Lib.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MyRemote.Lib.Action
{
    [KnownType(typeof(ListFilesAction))]
    [KnownType(typeof(WaitAction))]
    [KnownType(typeof(KeyboardAction))]
    public abstract class CommandAction : ISerializable
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();

        [JsonIgnore]
        public abstract string Code { get; }
        public Dictionary<string, string> Result { get; protected set; }


        public CommandAction(Dictionary<string, string> parameters)
        {
            if (parameters != null)
                foreach (var param in parameters)
                    Parameters.Add(param.Key, param.Value);
        }


        /*  public CommandAction(SerializationInfo info, StreamingContext context)
          {
              if (info == null)
                  throw new System.ArgumentNullException("info");

              Title = (string)info.GetValue("title", typeof(string));

              Parameters = (List<ActionParameter>)info.GetValue("params", typeof(List<ActionParameter>));
              Result = (Dictionary<string, string>)info.GetValue("result", typeof(Dictionary<string, string>));
          }
        */

        public string this[string name]
        {
            get
            {
                return Parameters[name];
            }
        }

        public virtual CommandRequest RequestThis()
        {
            return new CommandRequest
            {
                Id = this.Id + "_AUTO",
                ActionId = Id,
                Parameters = new Dictionary<string, string>(Parameters)
            };
        }

        public abstract CommandResponse Execute();

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("title", Title);
            info.AddValue("params", Parameters);
            info.AddValue("result", Result);
        }
        //SerializeBinary(info, context);
    }

}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DIZUCMD.Models.Blocks
{
    class Block
    {
        public string Username { get; set; }
        public DateTime Inicio { get; set; }
        public int Minutes { get; set; }
    }

    class Bloqueios
    {
        public List<Block> Blocks { get; set; }

        public Bloqueios ()
        {
            this.Blocks = new List<Block>();
        }

        public void LimparLista()
        {
            var aux = this;
            for (int i = 0; i < aux.Blocks.Count; i++)
            {
                TimeSpan dif = DateTime.Now - aux.Blocks[i].Inicio;
                if (dif.TotalMinutes > aux.Blocks[i].Minutes)
                {
                    this.Blocks.Remove(aux.Blocks[i]);
                }
            }
        }

        public void AdicionarBlock (string username, int minutes)
        {
            Block b = new Block
            {
                Inicio = DateTime.Now,
                Minutes = minutes,
                Username = username
            };
            this.Blocks.Add(b);
        }
    }
}

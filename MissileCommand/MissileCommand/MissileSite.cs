﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tesr
{
	class MissileCommand
	{
		//Building site;
		int missileStock;
		public MissileSite(/*Building b,*/ int stock)
		{
			//site=b;
			missileStock = stock;
		}
		public int getStock() { return missileStock; }

	}
}

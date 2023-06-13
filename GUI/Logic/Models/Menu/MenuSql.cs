using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logic.SQL;
using Npgsql;

namespace GUI.Logic.Models.Menu;

public class MenuSql {
	public async static Task<List<MenuType>> GetMenu(string type) {
		Library.Database db = new Library.Database();
		switch (type) {
			case "lunch":
				var cmd = new NpgsqlCommand("SELECT * FROM lunchmenu", db.Conn);
				var reader = await db.Query(cmd);
				await reader.ReadAsync();

				break;
			case "dinner":
				break;
			case "drinks":
				break;

		}

		// default empty
		return new List<MenuType>();
	}
}
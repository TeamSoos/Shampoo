using Logic.Models.Base;

namespace GUI.Logic.Models.Menu; 

public class MenuType : BaseType {
	public int ID;
	public string Type;
	public string Name;
	public decimal Price;
	
	private MenuType(int id) : base(id) {
	}
	
	

	public override T getByID<T>(int id) {
		throw new System.NotImplementedException();
	}
}
using ServicioInventario;

InventarioService Servicio = new();


Servicio.GetInventario();
Servicio.SetInventario();
Servicio.SaveJsonInventario();
Servicio.SendJsonInventario();

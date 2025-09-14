# 🏠 Proyecto Inmobiliaria

Sistema para la **gestión informatizada de alquileres de propiedades inmuebles** en una agencia inmobiliaria.  
Permite administrar propietarios, inmuebles, contratos, pagos e inquilinos, con distintos roles de usuario y generación de informes.

---

## 📌 Entidades principales
- **Propietario** → Dueño de uno o varios inmuebles.  
- **Inmueble** → Propiedades disponibles en alquiler (residenciales o comerciales).  
- **Inquilino** → Persona que alquila un inmueble mediante un contrato.  
- **Contrato** → Relaciona inquilino e inmueble, con monto y fechas de vigencia.  
- **Pago** → Registra los pagos mensuales de cada contrato.  
- **Usuario** → Acceso al sistema con email y contraseña.  
  - Roles: **Administrador** (gestiona usuarios y elimina entidades) y **Empleado** (gestiona su perfil).  

---

## ⚙️ Funcionalidades principales
- **Gestión de inmuebles**: registro, modificación, suspensión temporal, tipos de inmueble (casa, departamento, local, etc.).  
- **Gestión de propietarios**: alta con DNI, nombre, apellido y contacto.  
- **Gestión de inquilinos**: registro de datos personales y contacto.  
- **Contratos de alquiler**:  
  - Validación de fechas y disponibilidad de inmuebles.  
  - Renovación automática de contratos.  
  - Terminación anticipada con cálculo de multa (1 o 2 meses según tiempo transcurrido).  
- **Pagos**:  
  - Registro de número de pago, fecha, detalle e importe.  
  - Anulación de pagos mediante cambio de estado.  
  - Carga de multas y deudas pendientes.  
- **Usuarios y roles**:  
  - Empleados: gestión de su perfil, contraseña y avatar.  
  - Administradores: gestión de usuarios y entidades, auditoría de acciones.  

---

## 📊 Informes disponibles
- Listado de inmuebles y propietarios (filtrado por disponibilidad).  
- Inmuebles de un propietario específico.  
- Contratos vigentes (por fechas).  
- Contratos asociados a un inmueble.  
- Contratos que finalizan en 30, 60 o 90 días.  
- Pagos de un contrato (con opción de cargar nuevos).  
- Búsqueda de inmuebles disponibles en un rango de fechas.

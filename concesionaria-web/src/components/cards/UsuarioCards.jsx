import DataTableToolbar from "@/components/tables/DataTableToolbar";
import { UserCard } from "./UserCard";

export function UsuarioCards({ data, tipo, onToggle, onSearch, onEdit, onChangePassword, onToggleStatus }) {
  return (
    <div className="overflow-hidden rounded-xl border border-slate-300/80 bg-white shadow-[0_4px_16px_rgba(15,23,42,0.07)]">
      <DataTableToolbar tipo={tipo} setTipo={onToggle} onSearch={onSearch} />
      {data?.length ? (
        <div className="grid gap-3 p-4 pt-3 sm:grid-cols-2 xl:grid-cols-3">
          {data.map((usuario) => (
            <UserCard key={usuario.usuarioId} usuario={usuario} tipo={tipo} onEdit={onEdit} onChangePassword={onChangePassword} onToggleStatus={onToggleStatus} />
          ))}
        </div>
      ) : (
        <div className="flex h-32 items-center justify-center text-sm text-slate-500">No hay resultados para mostrar.</div>
      )}
    </div>
  );
}

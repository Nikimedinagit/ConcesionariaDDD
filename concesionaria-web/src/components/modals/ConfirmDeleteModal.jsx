import { AlertTriangle, Trash2 } from "lucide-react";
import { ModalCustom } from "./ModalCustom";

export function ConfirmDeleteModal({
  isOpen,
  onClose,
  onConfirm,
  loading = false,
  title = "Confirmar eliminación",
  description,
}) {
  return (
    <ModalCustom
      isOpen={isOpen}
      onClose={onClose}
      onSave={onConfirm}
      title={title}
      icon={Trash2}
      loading={loading}
      saveText="Eliminar"
      maxWidth="max-w-md"
      destructive
    >
      <div className="flex items-start gap-3 rounded-xl border border-red-200 bg-red-50 p-4">
        <AlertTriangle className="mt-0.5 h-5 w-5 shrink-0 text-red-600" />
        <div>
          <p className="text-sm font-semibold text-slate-800">{description}</p>
          <p className="mt-1 text-sm text-slate-600">
            Esta acción no se puede deshacer.
          </p>
        </div>
      </div>
    </ModalCustom>
  );
}

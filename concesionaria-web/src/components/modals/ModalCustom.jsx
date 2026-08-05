import { useEffect } from "react";

import { createPortal } from "react-dom";
import { motion, AnimatePresence } from "framer-motion";

import { X } from "lucide-react";

import { Button } from "@/components/ui/button";

export function ModalCustom({
  isOpen,
  onClose,
  onSave,
  title,
  icon: Icon,
  children,
  loading = false,
  saveText = "Guardar",
  cancelText = "Cancelar",
  maxWidth = "max-w-lg",
}) {

  useEffect(() => {

    const handleKeyDown = (e) => {

      if (e.key === "Escape") {
        e.preventDefault();

        if (isOpen) {
          onClose();
        }
      }

      if (e.key === "Enter") {
        e.preventDefault();

        if (isOpen && !loading) {
          onSave();
        }
      }
    };

    window.addEventListener("keydown", handleKeyDown);

    return () => {
      window.removeEventListener("keydown", handleKeyDown);
    };

  }, [isOpen, onClose, onSave, loading]);

  return createPortal(
    <AnimatePresence>
      {isOpen && (
        <div className="fixed inset-0 z-[10000] flex items-center justify-center p-4">

          <motion.div
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            exit={{ opacity: 0 }}
            onClick={onClose}
            className="fixed inset-0 bg-slate-900/40 backdrop-blur-sm"
          />

          <motion.div
            initial={{ scale: 0.95, opacity: 0, y: 20 }}
            animate={{ scale: 1, opacity: 1, y: 0 }}
            exit={{ scale: 0.95, opacity: 0, y: 20 }}
            className={`relative w-full ${maxWidth} overflow-hidden rounded-xl border border-slate-300 bg-white shadow-[0_22px_55px_-18px_rgba(15,23,42,0.35)]`}
          >

            <div className="flex items-center justify-between border-b border-slate-200 bg-[hsl(var(--nav-bg)/0.06)] p-3 sm:px-4">

              <div className="flex items-center gap-3">

                {Icon && (
                  <div
                    className="flex h-10 w-10 items-center justify-center rounded-xl text-white shadow-lg"
                    style={{
                      backgroundColor: "hsl(var(--nav-bg))",
                    }}
                  >
                    <Icon className="h-5 w-5" />
                  </div>
                )}

                <h3 className="text-xl font-black tracking-tight text-slate-900">
                  {title}
                </h3>

              </div>

              <button
                onClick={onClose}
                className="rounded-full p-2 text-slate-400 transition-colors hover:bg-slate-100 hover:text-slate-600"
              >
                <X className="h-5 w-5" />
              </button>

            </div>

            <div className="max-h-[70vh] overflow-y-auto bg-slate-100/70 p-3 sm:p-4">

              <div className="grid gap-5">
                {children}
              </div>

            </div>

            <div className="flex items-center justify-end gap-3 border-t border-slate-200 bg-slate-50 p-3 sm:px-4">

              <Button
                variant="outline"
                onClick={onClose}
                disabled={loading}
                className="
                  h-9 rounded-xl
                  border-slate-200
                  px-6 font-semibold
                  text-slate-500
                  hover:bg-slate-200
                  hover:text-slate-900
                "
              >
                {cancelText}
              </Button>

              <Button
                onClick={onSave}
                disabled={loading}
                className="
                  h-9 rounded-xl
                  px-6 font-semibold
                  text-white
                  transition-all
                  hover:opacity-90
                  active:scale-95
                  disabled:opacity-50
                "
                style={{
                  backgroundColor: "hsl(var(--nav-bg))",
                }}
              >
                {loading
                  ? "Procesando..."
                  : saveText}
              </Button>

            </div>

          </motion.div>

        </div>
      )}
    </AnimatePresence>,
    document.body
  );
}

import { Button } from "@/components/ui/button";

import {
  SquarePen,
  Trash2,
  RotateCcw,
} from "lucide-react";

export const ActionButton = ({
  type,
  onClick,
  className = "",
}) => {
  const configs = {
    edit: {
      icon: SquarePen,

      className: `
        text-sky-500
        hover:bg-sky-50
        hover:text-sky-600
      `,

      label: "Editar",
    },

    desactivar: {
      icon: Trash2,

      className: `
        text-red-500
        hover:bg-red-50
        hover:text-red-600
      `,

      label: "Desactivar",
    },

    activar: {
      icon: RotateCcw,

      className: `
        text-emerald-500
        hover:bg-emerald-50
        hover:text-emerald-600
      `,

      label: "Activar",
    },
  };

  const {
    icon: Icon,
    className: styles,
    label,
  } = configs[type];

  return (
    <Button
      variant="ghost"
      size="icon"
      onClick={onClick}
      title={label}
      className={`
        h-9 w-9
        rounded-lg
        transition-colors
        ${styles}
        ${className}
      `}
    >
      <Icon className="h-[18px] w-[18px]" />
    </Button>
  );
};
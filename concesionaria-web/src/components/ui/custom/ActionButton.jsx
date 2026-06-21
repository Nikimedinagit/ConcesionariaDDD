import { Button } from "@/components/ui/button";

import {
  SquarePen,
  Trash2,
  RotateCcw,
  KeyRound,
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
    },

    desactivar: {
      icon: Trash2,

      className: `
        text-red-500
        hover:bg-red-50
        hover:text-red-600
      `,
    },

    activar: {
      icon: RotateCcw,

      className: `
        text-emerald-500
        hover:bg-emerald-50
        hover:text-emerald-600
      `,
    },

    password: {
      icon: KeyRound,

      className: `
        text-amber-500
        hover:bg-amber-50
        hover:text-amber-600
      `,
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
        h-7.5 w-7.5
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

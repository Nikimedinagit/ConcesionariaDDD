import { motion } from "framer-motion";

const PageHeader = ({
  title,
  icon: Icon,
  children,
}) => {
  return (
    <motion.div
      initial={{ opacity: 0, y: -8 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.3 }}
      className="
        relative overflow-hidden
        rounded-xl
        border border-slate-200/70
        bg-white
        px-4 py-4
        shadow-sm
        mb-6 mt-3
      "
    >
      <div
        className="
          absolute -right-10 -top-10
          h-32 w-32
          rounded-full
          bg-[hsl(var(--nav-bg))]
          opacity-[0.03]
        "
      />

      <div
        className="
          relative
          flex flex-col gap-4
          lg:flex-row lg:items-center lg:justify-between
        "
      >
        <div className="flex items-center gap-3 min-w-0">
          
          {Icon && (
            <div
              className="
                flex h-10 w-10 shrink-0
                items-center justify-center
                rounded-xl
                bg-[hsl(var(--nav-bg))]
                text-white
                shadow-sm
              "
            >
              <Icon size={18} strokeWidth={2.2} />
            </div>
          )}

          <div className="min-w-0">
            <h1
              className="
                truncate
                text-[22px]
                font-bold
                leading-none
                tracking-tight
                text-slate-900

                sm:text-2xl
              "
            >
              {title}
            </h1>

            <div className="mt-2 flex items-center gap-1.5">
              <div
                className="
                  h-1 w-10 rounded-full
                  bg-[hsl(var(--nav-bg))]
                "
              />

              <div
                className="
                  h-1 w-4 rounded-full
                  bg-[hsl(var(--nav-bg))]
                  opacity-35
                "
              />
            </div>
          </div>
        </div>

        {children && (
          <div
            className="
              flex w-full flex-wrap items-center gap-2

              sm:w-auto
            "
          >
            {children}
          </div>
        )}
      </div>
    </motion.div>
  );
};

export default PageHeader;
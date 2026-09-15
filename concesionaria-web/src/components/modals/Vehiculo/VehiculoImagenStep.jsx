import { useEffect, useRef, useState } from "react";
import {
  ImagePlus,
  Images,
  ArrowLeft,
  ArrowRight,
  Star,
  Trash2,
  UploadCloud,
} from "lucide-react";
import { Button } from "@/components/ui/button";
import {
  MAX_VEHICULO_IMAGES,
  vehiculoImageFileSchema,
  vehiculoImagesSchema,
} from "@/validations/Vehiculo/vehiculoImagen.validation";

export function VehiculoImagenStep({
  images,
  onChange,
  onRemoveExisting,
  loading = false,
  externalError = "",
}) {
  const inputRef = useRef(null);
  const imagesRef = useRef(images);
  const [isDragging, setIsDragging] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    imagesRef.current = images;
  }, [images]);

  useEffect(() => {
    return () => {
      imagesRef.current.forEach((image) => URL.revokeObjectURL(image.preview));
    };
  }, []);

  const addFiles = (fileList) => {
    const files = Array.from(fileList || []);
    if (!files.length) return;

    const availableSlots = MAX_VEHICULO_IMAGES - images.length;
    if (availableSlots <= 0) {
      setError(`Podés cargar hasta ${MAX_VEHICULO_IMAGES} imágenes.`);
      return;
    }

    const validFiles = [];
    let validationError = "";

    for (const file of files) {
      const fileResult = vehiculoImageFileSchema.safeParse(file);

      if (!fileResult.success) {
        validationError = fileResult.error.issues[0].message;
        continue;
      }

      const isDuplicate = [...images, ...validFiles].some(
        (image) =>
          image.file?.name === file.name &&
          image.file?.size === file.size &&
          image.file?.lastModified === file.lastModified,
      );

      if (isDuplicate) {
        validationError = "Se omitieron imágenes duplicadas.";
        continue;
      }

      validFiles.push({
        id: crypto.randomUUID(),
        file,
        preview: URL.createObjectURL(file),
        isPrincipal: false,
      });
    }

    const acceptedFiles = validFiles.slice(0, availableSlots);
    validFiles.slice(availableSlots).forEach((image) =>
      URL.revokeObjectURL(image.preview),
    );

    if (validFiles.length > availableSlots) {
      validationError = `Solo se agregaron ${availableSlots} imágenes para respetar el límite de ${MAX_VEHICULO_IMAGES}.`;
    }

    if (acceptedFiles.length) {
      const nextImages = [...images, ...acceptedFiles];

      if (!nextImages.some((image) => image.isPrincipal)) {
        nextImages[0] = { ...nextImages[0], isPrincipal: true };
      }

      const imagesResult = vehiculoImagesSchema.safeParse(nextImages);

      if (imagesResult.success) {
        onChange(nextImages);
      } else {
        acceptedFiles.forEach((image) => URL.revokeObjectURL(image.preview));
        validationError = imagesResult.error.issues[0].message;
      }
    }

    setError(validationError);

    if (inputRef.current) inputRef.current.value = "";
  };

  const removeImage = (imageId) => {
    const imageToRemove = images.find((image) => image.id === imageId);
    if (imageToRemove?.preview?.startsWith("blob:")) {
      URL.revokeObjectURL(imageToRemove.preview);
    }

    if (imageToRemove?.isExisting) onRemoveExisting?.(imageToRemove.id);

    const remainingImages = images.filter((image) => image.id !== imageId);

    if (
      remainingImages.length &&
      !remainingImages.some((image) => image.isPrincipal)
    ) {
      remainingImages[0] = { ...remainingImages[0], isPrincipal: true };
    }

    onChange(remainingImages);
    setError("");
  };

  const moveImage = (imageId, direction) => {
    const currentIndex = images.findIndex((image) => image.id === imageId);
    const targetIndex = currentIndex + direction;

    if (currentIndex < 0 || targetIndex < 0 || targetIndex >= images.length) {
      return;
    }

    const reorderedImages = [...images];
    [reorderedImages[currentIndex], reorderedImages[targetIndex]] = [
      reorderedImages[targetIndex],
      reorderedImages[currentIndex],
    ];
    onChange(reorderedImages);
    setError("");
  };

  const setPrincipal = (imageId) => {
    onChange(
      images.map((image) => ({
        ...image,
        isPrincipal: image.id === imageId,
      })),
    );
  };

  const handleDrop = (event) => {
    event.preventDefault();
    setIsDragging(false);
    addFiles(event.dataTransfer.files);
  };

  return (
    <div className="space-y-4">
      <div className="flex flex-col gap-2 sm:flex-row sm:items-end sm:justify-between">
        <div>
          <div className="flex items-center gap-2 text-sm font-bold text-slate-800">
            <Images className="h-4 w-4 text-[hsl(var(--nav-bg))]" />
            Imágenes del vehículo
          </div>
          <p className="mt-1 text-sm text-slate-500">
            Agregá fotos claras del exterior, interior y detalles importantes.
          </p>
        </div>

        <span className="w-fit rounded-full bg-[hsl(var(--nav-bg)/0.1)] px-3 py-1.5 text-xs font-bold uppercase text-[hsl(var(--nav-bg))] ring-1 ring-[hsl(var(--nav-bg)/0.3)]">
          {images.length} / {MAX_VEHICULO_IMAGES} imágenes
        </span>
      </div>

      <button
        type="button"
        disabled={loading}
        onClick={() => inputRef.current?.click()}
        onDragEnter={(event) => {
          event.preventDefault();
          setIsDragging(true);
        }}
        onDragOver={(event) => event.preventDefault()}
        onDragLeave={() => setIsDragging(false)}
        onDrop={handleDrop}
        className={`group flex min-h-36 w-full flex-col items-center justify-center rounded-xl border-2 border-dashed px-6 py-5 text-center transition-all ${
          isDragging
            ? "border-[hsl(var(--nav-bg))] bg-[hsl(var(--nav-bg)/0.08)]"
            : "border-slate-300 bg-white hover:border-[hsl(var(--nav-bg)/0.45)] hover:bg-[hsl(var(--nav-bg)/0.03)]"
        }`}
      >
        <span className="flex h-11 w-11 items-center justify-center rounded-xl bg-[hsl(var(--nav-bg)/0.1)] text-[hsl(var(--nav-bg))] transition-transform group-hover:-translate-y-0.5">
          <UploadCloud className="h-5 w-5" />
        </span>
        <span className="mt-3 text-sm font-bold text-slate-800">
          Arrastrá imágenes o hacé clic para seleccionar
        </span>
        <span className="mt-1 text-sm text-slate-500">
          JPG, PNG o WebP · Máximo 5 MB por imagen
        </span>
      </button>

      <input
        ref={inputRef}
        type="file"
        accept="image/jpeg,image/png,image/webp"
        multiple
        className="hidden"
        onChange={(event) => addFiles(event.target.files)}
      />

      {(error || externalError) && (
        <p className="rounded-lg border border-red-200 bg-red-50 px-3 py-2 text-sm font-medium text-red-600">
          {error || externalError}
        </p>
      )}

      {loading && (
        <div className="flex h-24 items-center justify-center rounded-xl border border-slate-200 bg-white text-sm font-medium text-slate-500">
          Cargando imágenes...
        </div>
      )}

      {!loading && images.length > 0 ? (
        <div className="grid grid-cols-1 gap-3 sm:grid-cols-2 lg:grid-cols-4">
          {images.map((image, index) => (
            <article
              key={image.id}
              className={`group relative overflow-hidden rounded-xl border bg-white shadow-sm transition-all ${
                image.isPrincipal
                  ? "border-amber-300 ring-2 ring-amber-200"
                  : "border-slate-200 hover:border-slate-300"
              }`}
            >
              <div className="aspect-[4/3] overflow-hidden bg-slate-100">
                <img
                  src={image.preview}
                  alt={`Vista previa ${index + 1}`}
                  className="h-full w-full object-cover transition-transform duration-200 group-hover:scale-105"
                />
              </div>

              {image.isPrincipal && (
                <span className="absolute left-2 top-2 flex items-center gap-1 rounded-full bg-amber-50 px-2 py-1 text-[9px] font-black uppercase text-amber-700 shadow-sm ring-1 ring-amber-200">
                  <Star className="h-3 w-3 fill-current" /> Principal
                </span>
              )}

              <div className="p-2">
                <div className="grid grid-cols-4 gap-1.5">
                  <Button
                    type="button"
                    variant="ghost"
                    size="icon"
                    disabled={index === 0}
                    onClick={() => moveImage(image.id, -1)}
                    className="h-9 w-full bg-slate-50 text-slate-500 ring-1 ring-slate-200 hover:bg-slate-100 disabled:opacity-30"
                    aria-label="Mover imagen hacia atrás"
                    title="Mover hacia atrás"
                  >
                    <ArrowLeft />
                  </Button>

                  <Button
                    type="button"
                    variant="ghost"
                    size="icon"
                    disabled={index === images.length - 1}
                    onClick={() => moveImage(image.id, 1)}
                    className="h-9 w-full bg-slate-50 text-slate-500 ring-1 ring-slate-200 hover:bg-slate-100 disabled:opacity-30"
                    aria-label="Mover imagen hacia adelante"
                    title="Mover hacia adelante"
                  >
                    <ArrowRight />
                  </Button>

                  <Button
                    type="button"
                    variant="ghost"
                    size="icon"
                    disabled={image.isPrincipal}
                    onClick={() => setPrincipal(image.id)}
                    className={`h-9 w-full ring-1 ${
                      image.isPrincipal
                        ? "bg-amber-50 text-amber-600 ring-amber-200 opacity-100"
                        : "bg-slate-50 text-slate-500 ring-slate-200 hover:bg-amber-50 hover:text-amber-600 hover:ring-amber-200"
                    }`}
                    aria-label={`Marcar ${image.file?.name || image.nombreOriginal} como principal`}
                    title={image.isPrincipal ? "Imagen principal" : "Marcar como principal"}
                  >
                    <Star className={image.isPrincipal ? "fill-current" : ""} />
                  </Button>

                  <Button
                    type="button"
                    variant="ghost"
                    size="icon"
                    onClick={() => removeImage(image.id)}
                    className="h-9 w-full bg-slate-50 text-slate-500 ring-1 ring-slate-200 hover:bg-red-50 hover:text-red-600 hover:ring-red-200"
                    aria-label={`Eliminar ${image.file?.name || image.nombreOriginal}`}
                    title="Eliminar imagen"
                  >
                    <Trash2 />
                  </Button>
                </div>

                <p className="mt-2 text-center text-xs font-semibold text-slate-500">
                  {((image.file?.size || image.tamanioBytes) / 1024 / 1024).toFixed(1)} MB
                </p>
              </div>
            </article>
          ))}

          {images.length < MAX_VEHICULO_IMAGES && (
            <button
              type="button"
              onClick={() => inputRef.current?.click()}
              className="flex min-h-32 flex-col items-center justify-center rounded-xl border border-dashed border-slate-300 bg-white text-slate-400 transition-colors hover:border-[hsl(var(--nav-bg)/0.45)] hover:text-[hsl(var(--nav-bg))]"
            >
              <ImagePlus className="h-5 w-5" />
              <span className="mt-2 text-[11px] font-bold uppercase">
                Agregar más
              </span>
            </button>
          )}
        </div>
      ) : !loading ? (
        <div className="flex items-center gap-3 rounded-lg border border-slate-200 bg-white px-3 py-2.5 text-xs text-slate-500">
          <ImagePlus className="h-4 w-4 shrink-0 text-slate-400" />
          Las imágenes son opcionales. Podés agregarlas ahora o más adelante.
        </div>
      ) : null}
    </div>
  );
}

import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { z } from "zod";
import { Button } from "@/components/ui/button";
import {
	Dialog,
	DialogContent,
	DialogHeader,
	DialogTitle,
} from "@/components/ui/dialog";
import {
	Form,
	FormControl,
	FormField,
	FormItem,
	FormLabel,
	FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { jobsApi } from "@/lib/api/jobs";
import { Textarea } from "@/components/ui/textarea";
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from "@/components/ui/select";

const formSchema = z.object({
	jobTitle: z.string().min(1, "Job title is required"),
	company: z.string().min(1, "Company is required"),
	industry: z.string().optional(),
	companyOverview: z.string().optional(),
	location: z.string().optional(),
	whyIWantToWorkHere: z.string().optional(),
	status: z.string().default("Not Applied"),
	dateApplied: z.string().optional(),
	jobPostingLink: z.string().url().optional().or(z.literal("")),
	notes: z.string().optional(),
});

type FormValues = z.infer<typeof formSchema>;

interface Props {
	open: boolean;
	onOpenChange: (open: boolean) => void;
}

export function CreateJobDialog({ open, onOpenChange }: Props) {
	const queryClient = useQueryClient();
	const form = useForm<FormValues>({
		resolver: zodResolver(formSchema),
		defaultValues: {
			jobTitle: "",
			company: "",
			industry: "",
			companyOverview: "",
			location: "",
			whyIWantToWorkHere: "",
			status: "Not Applied",
			dateApplied: "",
			jobPostingLink: "",
			notes: "",
		},
	});

	const { mutate, isPending } = useMutation({
		mutationFn: (values: FormValues) => jobsApi.create(values),
		onSuccess: () => {
			queryClient.invalidateQueries({ queryKey: ["jobs"] });
			form.reset();
			onOpenChange(false);
		},
	});

	return (
		<Dialog open={open} onOpenChange={onOpenChange}>
			<DialogContent className="sm:max-w-[425px] max-h-[90vh] overflow-y-auto">
				<DialogHeader>
					<DialogTitle>Add New Job Application</DialogTitle>
				</DialogHeader>
				<Form {...form}>
					<form
						onSubmit={form.handleSubmit((values) => mutate(values))}
						className="space-y-4"
					>
						<FormField
							control={form.control}
							name="jobTitle"
							render={({ field }) => (
								<FormItem>
									<FormLabel>Job Title</FormLabel>
									<FormControl>
										<Input {...field} />
									</FormControl>
									<FormMessage />
								</FormItem>
							)}
						/>
						<FormField
							control={form.control}
							name="company"
							render={({ field }) => (
								<FormItem>
									<FormLabel>Company</FormLabel>
									<FormControl>
										<Input {...field} />
									</FormControl>
									<FormMessage />
								</FormItem>
							)}
						/>
						<FormField
							control={form.control}
							name="location"
							render={({ field }) => (
								<FormItem>
									<FormLabel>Location</FormLabel>
									<FormControl>
										<Input {...field} />
									</FormControl>
									<FormMessage />
								</FormItem>
							)}
						/>
						<FormField
							control={form.control}
							name="jobPostingLink"
							render={({ field }) => (
								<FormItem>
									<FormLabel>Job Posting Link</FormLabel>
									<FormControl>
										<Input {...field} />
									</FormControl>
									<FormMessage />
								</FormItem>
							)}
						/>
						<FormField
							control={form.control}
							name="notes"
							render={({ field }) => (
								<FormItem>
									<FormLabel>Notes</FormLabel>
									<FormControl>
										<Input {...field} />
									</FormControl>
									<FormMessage />
								</FormItem>
							)}
						/>
						<FormField
							control={form.control}
							name="industry"
							render={({ field }) => (
								<FormItem>
									<FormLabel>Industry</FormLabel>
									<FormControl>
										<Input {...field} />
									</FormControl>
									<FormMessage />
								</FormItem>
							)}
						/>
						<FormField
							control={form.control}
							name="companyOverview"
							render={({ field }) => (
								<FormItem>
									<FormLabel>Company Overview</FormLabel>
									<FormControl>
										<Textarea
											placeholder="What does the company do? What's their mission?"
											className="min-h-[100px]"
											{...field}
										/>
									</FormControl>
									<FormMessage />
								</FormItem>
							)}
						/>
						<FormField
							control={form.control}
							name="whyIWantToWorkHere"
							render={({ field }) => (
								<FormItem>
									<FormLabel>Why I Want To Work Here</FormLabel>
									<FormControl>
										<Textarea
											placeholder="What interests you about this company?"
											className="min-h-[100px]"
											{...field}
										/>
									</FormControl>
									<FormMessage />
								</FormItem>
							)}
						/>
						<FormField
							control={form.control}
							name="status"
							render={({ field }) => (
								<FormItem>
									<FormLabel>Status</FormLabel>
									<Select
										onValueChange={field.onChange}
										defaultValue={field.value}
									>
										<FormControl>
											<SelectTrigger>
												<SelectValue placeholder="Select application status" />
											</SelectTrigger>
										</FormControl>
										<SelectContent>
											<SelectItem value="Not Applied">Not Applied</SelectItem>
											<SelectItem value="Applied">Applied</SelectItem>
											<SelectItem value="Interview">Interview</SelectItem>
											<SelectItem value="Rejected">Rejected</SelectItem>
											<SelectItem value="Offer">Offer</SelectItem>
										</SelectContent>
									</Select>
									<FormMessage />
								</FormItem>
							)}
						/>
						<FormField
							control={form.control}
							name="dateApplied"
							render={({ field }) => (
								<FormItem>
									<FormLabel>Date Applied</FormLabel>
									<FormControl>
										<Input type="date" {...field} value={field.value || ""} />
									</FormControl>
									<FormMessage />
								</FormItem>
							)}
						/>
						<Button type="submit" disabled={isPending}>
							{isPending ? "Submitting..." : "Submit"}
						</Button>
					</form>
				</Form>
			</DialogContent>
		</Dialog>
	);
}

import { useQuery } from "@tanstack/react-query";
import { jobsApi } from "@/lib/api/jobs";
import { Button } from "@/components/ui/button";
import {
	Card,
	CardContent,
	CardDescription,
	CardFooter,
	CardHeader,
	CardTitle,
} from "@/components/ui/card";
import type { Job } from "@/types/job";
import { CreateJobDialog } from "./CreateJobDialog";
import { useState } from "react";
import { Badge, type BadgeProps } from "@/components/ui/badge";
import { CalendarIcon, BuildingIcon } from "lucide-react";

export function JobList() {
	const [isCreateOpen, setIsCreateOpen] = useState(false);
	const { data: jobs, isLoading } = useQuery({
		queryKey: ["jobs"],
		queryFn: () => jobsApi.getAll(),
	});

	if (isLoading) {
		return <div>Loading...</div>;
	}

	return (
		<div className="p-4">
			<div className="flex justify-between items-center mb-6">
				<h1 className="text-2xl font-bold">My Job Applications</h1>
				<Button onClick={() => setIsCreateOpen(true)}>
					Add New Application
				</Button>
			</div>

			<div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
				{jobs?.map((job: Job) => (
					<Card key={job.id} className="hover:shadow-lg transition-shadow">
						<CardHeader>
							<div className="flex justify-between items-start">
								<div>
									<CardTitle className="text-xl">{job.jobTitle}</CardTitle>
									<CardDescription className="flex items-center gap-1 mt-1">
										<BuildingIcon className="h-4 w-4" />
										{job.company}
									</CardDescription>
								</div>
								<Badge variant={getStatusVariant(job.status)}>
									{job.status}
								</Badge>
							</div>
						</CardHeader>
						<CardContent className="space-y-2">
							<div className="flex items-center gap-2 text-sm text-muted-foreground">
								<CalendarIcon className="h-4 w-4" />
								Applied: {new Date(job.dateApplied).toLocaleDateString()}
							</div>
						</CardContent>
						<CardFooter>
							<Button variant="outline" className="w-full">
								View Details
							</Button>
						</CardFooter>
					</Card>
				))}
			</div>

			<CreateJobDialog open={isCreateOpen} onOpenChange={setIsCreateOpen} />
		</div>
	);
}

function getStatusVariant(status: string): BadgeProps["variant"] {
	switch (status.toLowerCase()) {
		case "applied":
			return "applied";
		case "interviewing":
			return "interviewing";
		case "offered":
			return "offered";
		case "rejected":
			return "rejected";
		default:
			return "pending";
	}
}
